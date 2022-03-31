using MovieHub.Models;

namespace MovieHub.ViewModels;

public class CreateMovieViewModel
{
    public Movie Movie { get; set; }
    public List<Pegi> Pegi { get; set; }
    public List<Genre> Type { get; set; }
}