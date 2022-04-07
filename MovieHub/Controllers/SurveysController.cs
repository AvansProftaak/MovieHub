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
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveysController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> SurveyOverview()
        {
            return View(await GetSurveyResponses());
        }

        public async Task<IEnumerable<Survey>> GetSurveyResponses()
        {
            return await _context.Survey.ToListAsync();
        }

        // GET: Surveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Age,Gender,Name,Email,Facilities,Hygiene,FoodDrinks,Staff,ScreenQuality,SoundQuality,Price,Remark,SurveyFilledIn")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                survey.SurveyFilledIn = DateTime.UtcNow;
                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThanksScreen));
            }
            return View(survey);
        }

        public IActionResult ThanksScreen()
        {
            return View();
        }
    }
}
