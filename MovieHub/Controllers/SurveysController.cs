using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class SurveysController : Controller
{
    // GET
    public IActionResult CreateSurvey()
    {
        return View();
    }
}