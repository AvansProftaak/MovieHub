using MovieHub.Models;

namespace MovieHub.ViewModels;

public class MovieRuntimeViewModel
{
    public List<MovieRuntime> RuntimeList { get; set; }
    public MovieRuntime MovieRuntime { get; set; }
    public Movie Movie { get; set; }
}