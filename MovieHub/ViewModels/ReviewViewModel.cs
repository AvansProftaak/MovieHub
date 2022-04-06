using MovieHub.Models;

namespace MovieHub.ViewModels;

public class ReviewViewModel
{
    public Review Review { get; set; }
    public List<Cinema> Cinemas { get; set; }
    public List<Hall> Halls { get; set; }
}