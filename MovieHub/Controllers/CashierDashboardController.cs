using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.ViewModels;

namespace MovieHub.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]

    public class CashierDashboardController : Controller
    {
        private readonly ILogger<CashierDashboardController> _logger;
        private readonly ApplicationDbContext _context;

        public CashierDashboardController(ILogger<CashierDashboardController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            CashierViewModel cashierViewModel = new CashierViewModel()
            {
                tickets = _context.Ticket.ToList(),
                seats = _context.Seat.ToList(),
                orders = _context.Order.ToList()
            };
            return View(cashierViewModel);
        }


        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Order.FindAsync(orderId);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            var ticket = await _context.Ticket.FindAsync(ticketId);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeSeat(int row, int seat, int ticketId, int showTimeId)
        {
            var showtime = await _context.Showtime.FindAsync(showTimeId);
            int? seatId = _context.Seat
                .Where(s => s.RowNumber == row)
                .Where(s => s.SeatNumber == seat)
                .Where(s => s.HallId == showtime.HallId)
                .ToList().FirstOrDefault().Id;
            var ticket = await _context.Ticket.FirstOrDefaultAsync(x => x.Id == ticketId);
            ticket.SeatId = seatId;
            _context.Ticket.Update(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
