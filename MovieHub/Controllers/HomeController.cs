﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModel;
using Npgsql;

namespace MovieHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ActionResult<IndexViewModel>> Index()
    {
        IndexViewModel indexViewModel = new IndexViewModel();
        
        indexViewModel.MovieIndex = MovieIndex();
        indexViewModel.Halls = GetHall();
        indexViewModel.ShowNext = ShowNext();
        indexViewModel.ShowNow = ShowNow();

        return View(indexViewModel);
        }

    public IOrderedQueryable<Showtime> MovieIndex()
    {
        return _context.Showtime
            .Where(s => s.StartAt.Date.Equals(DateTime.Today))
            .Where(s => s.StartAt.ToLocalTime() > DateTime.Now)
            .Include(s => s.Hall)
            .Include(s => s.Movie)
            .OrderBy(s => s.StartAt);
    }

    public List<Hall> GetHall()
    {
        return _context.Hall
            .FromSqlRaw("SELECT * FROM rob.public.\"Hall\"").ToList();
    }
    
    public List<Showtime> ShowNext()
    {
        return _context.Showtime!
                 .FromSqlRaw("SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\") rn FROM rob.public.\"Showtime\" where \"StartAt\" > now()) x JOIN rob.public.\"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }

    public List<Showtime> ShowNow()
    {
        return _context.Showtime!
            .FromSqlRaw(
                "SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\" DESC ) rn FROM rob.public.\"Showtime\" WHERE \"StartAt\" < now()) x JOIN rob.public.\"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }
    
    //
    // public IEnumerable<Showtime> MovieNow(int hallId)
    // {
    //     return _context.Showtime
    //         .FromSqlRaw(
    //             "SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\") rn FROM rob.public.\"Showtime\" where \"HallId\" = {0} and \"StartAt\" > now()) x JOIN rob.public.\"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"",
    //             hallId);
    // }
    
    public async Task<IActionResult> SearchForMovie(string searchPhrase)
    {
        var applicationDbContext = _context.Showtime.Where(s => 
                s.StartAt.Date.Equals(DateTime.Today)).Where(s => 
                s.StartAt.ToLocalTime() > DateTime.Now).Include(s => s.Hall)
            .Include(s => s.Movie).Where(s=>s.Movie.Title.Contains(searchPhrase)).OrderBy(s => s.StartAt);

        return View("Index", applicationDbContext.ToList());
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}