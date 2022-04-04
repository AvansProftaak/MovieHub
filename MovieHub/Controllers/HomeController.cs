﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ActionResult<IndexViewModel>> Index()
    {
        var indexViewModel = new IndexViewModel
        {
            Halls = _context.Hall.OrderBy(h => h.Id).ToList(),
            Movies = _context.Movie.OrderBy(m => m.Id).ToList(),
            Showtimes = _context.Showtime.Include(s => s.Hall).Include(s => s.Movie).ToList(),
            MovieRuntimes = _context.MovieRuntime.ToList()
        };

        return Task.FromResult<ActionResult<IndexViewModel>>(View(indexViewModel));
        }
    

    public List<Showtime> ShowNext()
    {
        return _context.Showtime!
                 .FromSqlRaw("SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\") rn FROM \"Showtime\" where \"StartAt\" > now()) x JOIN \"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }

    public List<Showtime> ShowNow()
    {
        return _context.Showtime!
            .FromSqlRaw(
                "SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\" DESC ) rn FROM \"Showtime\" WHERE \"StartAt\" < now()) x JOIN \"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    private static DateTime GetFirstDayOfWeek(DateTime date)
    {
        const DayOfWeek firstDay = DayOfWeek.Monday;

        var diff = date.DayOfWeek - firstDay;
        if (diff < 0)
            diff += 7;
        return date.AddDays(-diff).Date;
    }

    //To Get The Last Day of the Week in C#
    private static DateTime GetLastDayOfWeek(DateTime date)
    {
        const DayOfWeek firstDay = DayOfWeek.Monday;
        var diff = date.DayOfWeek - firstDay;

        if (diff < 0)
            diff += 7;
        var start = date.AddDays(-diff).Date;
        
        // Add 6 days to get the last day, but to display all movies from the last day we add 7!
        return start.AddDays(7).Date;
    }
    
    public async Task<IActionResult> InsertEmail(string email)
    {
        var result = await _context.Newsletter.FirstOrDefaultAsync(p => p.Email == email);
        
        if(result != null) 
        {
            return BadRequest();
        } 
        
        var newsletter = new Newsletter 
        {
            Email = email,
        };
        
        await _context.Newsletter.AddAsync(newsletter);
        await _context.SaveChangesAsync();
        return Ok();
    }
}