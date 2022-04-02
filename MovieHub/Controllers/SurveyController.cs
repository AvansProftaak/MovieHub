#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin, Manager")]
        // GET: Survey
        public IActionResult Index()
        {

            var indexSurveyViewModel = new IndexSurveyViewModel
            {
                Halls = _context.Hall.OrderBy(h => h.Name).ToList(),
                Surveys = _context.Survey.ToList()
            };
            
            return View(indexSurveyViewModel);
        }

        [Authorize(Roles = "Admin, Manager")]
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
                HallList = _context.Hall.OrderBy(h => h.Name).ToList(),
                Survey = new Survey()
            };
            
            return View(createSurveyViewModel);
        }

        
        // POST: Survey/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "CinemaNumber,TicketPrice,ScreenQuality,SoundQuality,PopcornQuality,Nuisance,Hygiene,ToiletHeight,Name,Email")]
            Survey survey)
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

        public IActionResult SurveysPerHall(int hallId)
        {

            return View();
        }
        
    }
}
