using MovieHub.Models;

namespace MovieHub.ViewModels;

public class CreateMovieViewModel
{
    public Movie Movie { get; set; }
    public List<Pegi> Pegis { get; set; }
    public List<Genre> Genres { get; set; }
}