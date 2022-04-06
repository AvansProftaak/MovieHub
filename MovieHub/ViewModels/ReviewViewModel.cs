using MovieHub.Models;

namespace MovieHub.ViewModels;

public class ReviewViewModel
{
    public Review Review { get; set; }
    public List<Cinema> Cinema { get; set; }
    public List<Hall> Hall { get; set; }
}