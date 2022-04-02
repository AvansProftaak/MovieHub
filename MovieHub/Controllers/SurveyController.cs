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
using MovieHub.ViewModels;

namespace MovieHub
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Survey
        public async Task<IActionResult> Index()
        {
            return View(await _context.Survey.ToListAsync());
        }

        // GET: Survey/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey
                .FirstOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // GET: Survey/Create
        public IActionResult Create()
        {

            var createSurveyViewModel = new CreateSurveyViewModel
            {
                HallList = new List<SelectListItem>(),
                Survey = new Survey()
            };
            
            
            foreach (var hall in GetHalls().Select(item => new SelectListItem()
                     {
                         Value = item.Id.ToString(),
                         Text = item.Name
                     }))
            {
                createSurveyViewModel.HallList.Add(hall);
            }
            
            

            return View(createSurveyViewModel);
        }
        
        public List<Hall> GetHalls()
        {
            var halls = _context.Hall?
                .FromSqlRaw("SELECT * FROM \"Hall\" ORDER BY \"Name\"").ToList();

            return halls;
        }

        // POST: Survey/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CinemaNumber,TicketPrice,ScreenQuality,SoundQuality,PopcornQuality,Nuisance,Hygiene,ToiletHeight,Name,Email")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                var newSurvey = new Survey
                {
                    CinemaNumber = survey.CinemaNumber,
                    TicketPrice = survey.TicketPrice,
                    ScreenQuality = survey.ScreenQuality,
                    SoundQuality = survey.SoundQuality,
                    PopcornQuality = survey.PopcornQuality,
                    Nuisance = survey.Nuisance,
                    Hygiene = survey.Hygiene,
                    ToiletHeight = survey.ToiletHeight,
                    Name = survey.Name,
                    Email = survey.Email,
                    TimeStamp = DateTime.UtcNow.AddHours(2)
                };
                
                _context.Add(newSurvey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Survey/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // POST: Survey/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CinemaNumber,TicketPrice,ScreenQuality,SoundQuality,PopcornQuality,Nuisance,Hygiene,ToiletHeight,Name,Email,TimeStamp")] Survey survey)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
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
            return View(survey);
        }

        // GET: Survey/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey
                .FirstOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // POST: Survey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Survey.FindAsync(id);
            _context.Survey.Remove(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyExists(int id)
        {
            return _context.Survey.Any(e => e.Id == id);
        }
    }
}
