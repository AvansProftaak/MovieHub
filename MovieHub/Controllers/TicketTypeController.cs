#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;

namespace MovieHub.Controllers
{
    public class TicketTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketType
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tickettype.ToListAsync());
        }

        // GET: TicketType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickettype = await _context.Tickettype
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickettype == null)
            {
                return NotFound();
            }

            return View(tickettype);
        }

        // GET: TicketType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Tickettype tickettype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tickettype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tickettype);
        }

        // GET: TicketType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickettype = await _context.Tickettype.FindAsync(id);
            if (tickettype == null)
            {
                return NotFound();
            }
            return View(tickettype);
        }

        // POST: TicketType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description")] Tickettype tickettype)
        {
            if (id != tickettype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tickettype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TickettypeExists(tickettype.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tickettype);
        }

        // GET: TicketType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickettype = await _context.Tickettype
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickettype == null)
            {
                return NotFound();
            }

            return View(tickettype);
        }

        // POST: TicketType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tickettype = await _context.Tickettype.FindAsync(id);
            _context.Tickettype.Remove(tickettype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TickettypeExists(int id)
        {
            return _context.Tickettype.Any(e => e.Id == id);
        }

        public static Tickettype GetNormalTicket(ApplicationDbContext context)
        {
            
            IEnumerable<Tickettype> ticket =  context.Tickettype
                .Where(t => t.Name.Equals("Normal"));

            return ticket.ToList().FirstOrDefault();
        }
        
        public static decimal GetNormalPrice(ApplicationDbContext context)
        {
            Tickettype normalTicket = new Tickettype();
            normalTicket = GetNormalTicket(context);
            
            return normalTicket.Price;
        }
        
        public static Tickettype Get3DTicket(ApplicationDbContext context)
        {
            IEnumerable<Tickettype> ticket =  context.Tickettype
                .Where(t => t.Name.Equals("3D"));

            return ticket.ToList().FirstOrDefault();
        }
        
        public static decimal Get3DPrice(ApplicationDbContext context)
        {
            Tickettype normalTicket = Get3DTicket(context);
            
            return normalTicket.Price;
        }
        
        public static decimal PriceCalculations(Tickettype ticket, int movieId, ApplicationDbContext context, bool normalPriceRaised)
        {
            /* writing this we had some issues with ticket price of the normal ticket
             * when the normal ticket passed the = > 90 mins or = 3D the values are raised
             * Due to the nature of our calculations it stays raised
             * so we have to figure out if normal ticket already passed the calculations or not
             * we could simply tell the owner to insert the normal ticketprices first but they might forget.... 
             */

            var movie = context.Movie.FirstOrDefault(m => m.Id == movieId);

            var normalPrice = GetNormalPrice(context);
            var price = normalPrice;
            
            if ((movie.Duration > 120) & (!normalPriceRaised) )
            {
                price += (decimal).50;
                
            }

            if (movie.Is3D & !normalPriceRaised)
            {
                price += Get3DPrice(context);
            }
            
            if (ticket.Name == "Normal")
            {
                return price;
            }

            return price - ticket.Price;
        }
    } 
}
