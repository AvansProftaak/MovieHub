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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
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

    public List<Movie> GetMovies()
    {
        return _context.Movie.ToList();
    }
}