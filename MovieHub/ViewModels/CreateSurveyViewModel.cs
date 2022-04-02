using Microsoft.AspNetCore.Mvc.Rendering;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class CreateSurveyViewModel
{
    public Hall? Hall { get; set; }
    public List<SelectListItem>? HallList { get; set; }
    public Survey Survey { get; set; }
}