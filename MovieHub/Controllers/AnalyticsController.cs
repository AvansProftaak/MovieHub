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
    
    public async Task<IActionResult> MovieRevenue(AnalyticsViewModel model)
    {
        if (model.startDate == DateTime.MinValue && model.endDate == DateTime.MinValue)
        {
            model.startDate = DateTime.Today;
            model.endDate = DateTime.Today;
        }
        
        var sql = "SELECT m.\"Title\" AS movieTitle, COUNT(DISTINCT s.\"Id\") AS amountShows, CONCAT('€',CAST(COALESCE((SELECT SUM(t2.\"Price\") FROM public.\"Ticket\" AS t2 JOIN public.\"Order\" AS o2 ON o2.\"Id\" = t2.\"OrderId\" JOIN public.\"Showtime\" AS s2 ON s2.\"Id\" = o2.\"ShowtimeId\" JOIN public.\"Movie\" AS m2 ON m2.\"Id\" = s2.\"MovieId\" WHERE m2.\"Id\" = m.\"Id\" AND DATE(s2.\"StartAt\") BETWEEN @startDate AND @endDate AND t2.\"SeatId\" IS NOT NULL),0) AS DECIMAL(16,2))) AS ticketRevenue, CONCAT('€',CAST(COALESCE((SELECT SUM(t2.\"Price\") FROM public.\"Ticket\" AS t2 JOIN public.\"Order\" AS o2 ON o2.\"Id\" = t2.\"OrderId\" JOIN public.\"Showtime\" AS s2 ON s2.\"Id\" = o2.\"ShowtimeId\" JOIN public.\"Movie\" AS m2 ON m2.\"Id\" = s2.\"MovieId\" WHERE m2.\"Id\" = m.\"Id\" AND DATE(s2.\"StartAt\") BETWEEN @startDate AND @endDate AND t2.\"SeatId\" IS NULL),0) AS DECIMAL(16,2))) AS arrangementRevenue, CONCAT('€',CAST(COALESCE(SUM(t.\"Price\"),0) AS DECIMAL(16,2))) AS totalRevenue FROM public.\"Movie\" AS m JOIN public.\"Showtime\" AS s ON s.\"MovieId\" = m.\"Id\"JOIN public.\"Order\" AS o ON o.\"ShowtimeId\" = s.\"Id\"JOIN public.\"Ticket\" AS t ON t.\"OrderId\" = o.\"Id\"WHERE DATE(s.\"StartAt\") BETWEEN @startDate AND @endDate GROUP BY m.\"Id\",m.\"Title\"";
        
        using (var connection = new NpgsqlConnection(CONNECTION_STRING))
        {
            var result = await connection.QueryAsync<MovieRevenue>(sql, new {startDate = model.startDate, endDate = model.endDate});
            var vm = new AnalyticsViewModel
            {
                startDate = model.startDate,
                endDate = model.endDate,
                MovieStatistics = result
            };
            return View(vm);
        }
    }
}