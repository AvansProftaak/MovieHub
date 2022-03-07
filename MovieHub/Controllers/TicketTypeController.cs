#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            Tickettype normalTicket = GetNormalTicket(context);
            
            
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
        
        public static decimal PriceCalculations(Tickettype ticket, Movie movie,ApplicationDbContext context)
        {
            
             
            
            decimal normalPrice = GetNormalPrice(context);
            decimal price = normalPrice;
            Console.WriteLine(price);
            Console.WriteLine(normalPrice);
            
            if (movie.Duration > 90)
            {
                price += (decimal).50;
            }

            if (movie.Is3D)
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
