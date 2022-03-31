using MovieHub.Models;

namespace MovieHub.ViewModels;

public class EditMovieViewModel
{
    public Movie Movie { get; set; }
    public List<Pegi> Pegis { get; set; }
    public List<Genre> Genre { get; set; }
}