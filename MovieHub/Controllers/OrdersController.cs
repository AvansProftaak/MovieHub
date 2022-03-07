using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    
    private readonly ILogger<OrdersController> _logger;
    private readonly ApplicationDbContext _context;

    public OrdersController(ILogger<OrdersController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Payment(int orderId)
    {
        var payment = await _context.Payment
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
        return View(payment);
    }
}