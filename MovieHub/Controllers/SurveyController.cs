#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var survey = GetSurvey(id);
            
            return View(await survey);
        }

        [HttpGet]
        public async Task<Survey> GetSurvey(int id)
        {
            return await _context.Survey.FirstAsync(m => m.Id == id);
        }
        
        
        public IActionResult Create()
        {

            var createSurveyViewModel = new CreateSurveyViewModel
            {
                HallList = _context.Hall.OrderBy(h => h.Name).ToList(),
                Survey = new Survey()
            };
            
            return View(createSurveyViewModel);
        }
        
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

                await _context.AddAsync(newSurvey);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> SurveysPerHall(int id)
        {
            var hall = _context.Hall.FirstOrDefault(h => h.Id == id)!;
            
            var surveysViewModel = new SurveysViewModel
            {
                Hall = hall,
                SurveyList = _context.Survey.Where(s => s.CinemaNumber == hall.Name).ToList()
            };
            
            return View(surveysViewModel);
        }
    }
}
