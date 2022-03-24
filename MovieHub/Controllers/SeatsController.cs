using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.ViewModels;

namespace MovieHub.Controllers
{
    public class SeatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seats
        public async Task<IActionResult> Index(int showtimeId)
        {
            var show = _context.Showtime?.First(s => s.Id == showtimeId);
            var hallId = show?.HallId;
            var seatViewModel = new SeatViewModel();
            
            var seatsPerHall = _context.Seat.Where(s => s.HallId == hallId).Include(s => s.Hall);
            seatViewModel.Seats = await seatsPerHall.ToListAsync();

            return View(seatViewModel);
        }
    }
}