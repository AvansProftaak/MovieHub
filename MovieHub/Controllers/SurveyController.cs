#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
using NUnit.Framework;

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
        public async Task<IActionResult> Details(int id)
        {
            var survey = GetSurveyAsync(id);
            
            return View(await survey);
        }

        [HttpGet]
        public async Task<Survey> GetSurveyAsync(int id)
        {
            return await _context.Survey.FirstAsync(m => m.Id == id);
        }

        [HttpGet]
        public async Task<IList<Survey>> GetSurveysAsync()
        {
            return await _context.Survey.ToListAsync();
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

                await CreateSurveyAsync(newSurvey);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        public async Task CreateSurveyAsync(Survey survey)
        {
            await _context.AddAsync(survey);
            await _context.SaveChangesAsync();
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
