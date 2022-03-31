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
    public class LostAndFoundController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LostAndFoundController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Show LostAndFound not collected
        public async Task<IActionResult> Index()
        {
            // return View(await _context.LostAndFound.ToListAsync());
            var lostAndFoundNotCollected = _context.LostAndFound
                .Where(m => m.Collected == false);
  
            return View(lostAndFoundNotCollected.ToList());
        }

        // GET: LostAndFound/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostAndFound = await _context.LostAndFound
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lostAndFound == null)
            {
                return NotFound();
            }

            return View(lostAndFound);
        }

        // GET: LostAndFound/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LostAndFound/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IssueDate,Find,Description,Collected")] LostAndFound lostAndFound)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lostAndFound);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lostAndFound);
        }

        // GET: LostAndFound/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostAndFound = await _context.LostAndFound.FindAsync(id);
            if (lostAndFound == null)
            {
                return NotFound();
            }
            return View(lostAndFound);
        }

        // POST: LostAndFound/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IssueDate,Find,Description,Collected")] LostAndFound lostAndFound)
        {
            if (id != lostAndFound.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lostAndFound);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostAndFoundExists(lostAndFound.Id))
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
            return View(lostAndFound);
        }

        // GET: LostAndFound/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostAndFound = await _context.LostAndFound
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lostAndFound == null)
            {
                return NotFound();
            }

            return View(lostAndFound);
        }

        // POST: LostAndFound/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostAndFound = await _context.LostAndFound.FindAsync(id);
            _context.LostAndFound.Remove(lostAndFound);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LostAndFoundExists(int id)
        {
            return _context.LostAndFound.Any(e => e.Id == id);
        }
    }
}
