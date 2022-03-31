using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
using Npgsql;

namespace MovieHub.Controllers;

[Authorize]
public class AnalyticsController : Controller
{
    private readonly ApplicationDbContext _context;
    private const string CONNECTION_STRING = "host=db-postgresql-ams3-54359-do-user-9452846-0.b.db.ondigitalocean.com;port=25060;username=doadmin;password=l22MGuwQCl314eL7;database=lars;IncludeErrorDetail=true";
    
    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> HallAnalytics(AnalyticsViewModel model)
    {
        if (model.startDate == DateTime.MinValue && model.endDate == DateTime.MinValue)
        {
            model.startDate = DateTime.Today;
            model.endDate = DateTime.Today;
        }
        
       var sql = "SELECT A.\"Name\" AS hallName, A.\"Title\" AS movieTitle, A.showtime, A.hallCapacity, A.seatsTaken, (A.hallCapacity - A.seatsTaken) AS seatsFree FROM (SELECT h.\"Name\", m.\"Title\", s.\"StartAt\" AS showtime, (SELECT COUNT(*) FROM public.\"Seat\" WHERE \"Seat\".\"HallId\" = h.\"Id\") AS hallCapacity, COUNT(t.\"Id\") AS seatsTaken FROM public.\"Ticket\" AS t JOIN public.\"Order\" AS o ON o.\"Id\" = t.\"OrderId\" JOIN public.\"Showtime\" AS s ON s.\"Id\" = o.\"ShowtimeId\" JOIN public.\"Movie\" AS m ON m.\"Id\" = s.\"MovieId\" JOIN public.\"Hall\" AS h ON h.\"Id\" = s.\"HallId\" WHERE DATE(s.\"StartAt\") BETWEEN @startDate AND @endDate AND t.\"SeatId\" IS NOT NULL GROUP BY h.\"Id\", h.\"Name\", m.\"Title\", s.\"StartAt\") AS A";
        
        using (var connection = new NpgsqlConnection(CONNECTION_STRING))
        {
            var result = await connection.QueryAsync<HallAnalytics>(sql, new {startDate = model.startDate, endDate = model.endDate});
            var vm = new AnalyticsViewModel
            {
                startDate = model.startDate,
                endDate = model.endDate,
                Statistics = result
            };
            return View(vm);
        }
    }
}