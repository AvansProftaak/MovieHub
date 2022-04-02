using Microsoft.AspNetCore.Mvc.Rendering;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class CreateSurveyViewModel
{
    public List<Hall>? HallList { get; set; }
    public Survey Survey { get; set; }
}