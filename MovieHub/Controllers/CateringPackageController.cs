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
    public class CateringPackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CateringPackageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CateringPackage
        public async Task<IActionResult> Index()
        {
            return View(await _context.CateringPackage.ToListAsync());
        }

        // GET: CateringPackage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cateringPackage = await _context.CateringPackage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cateringPackage == null)
            {
                return NotFound();
            }

            return View(cateringPackage);
        }

        // GET: CateringPackage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CateringPackage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Description,Amount")] CateringPackage cateringPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cateringPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cateringPackage);
        }

        // GET: CateringPackage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cateringPackage = await _context.CateringPackage.FindAsync(id);
            if (cateringPackage == null)
            {
                return NotFound();
            }
            return View(cateringPackage);
        }

        // POST: CateringPackage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Amount")] CateringPackage cateringPackage)
        {
            if (id != cateringPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cateringPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CateringPackageExists(cateringPackage.Id))
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
            return View(cateringPackage);
        }

        // GET: CateringPackage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cateringPackage = await _context.CateringPackage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cateringPackage == null)
            {
                return NotFound();
            }

            return View(cateringPackage);
        }

        // POST: CateringPackage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cateringPackage = await _context.CateringPackage.FindAsync(id);
            _context.CateringPackage.Remove(cateringPackage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CateringPackageExists(int id)
        {
            return _context.CateringPackage.Any(e => e.Id == id);
        }
    }
}
