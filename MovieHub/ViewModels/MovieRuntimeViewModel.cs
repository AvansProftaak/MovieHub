using MovieHub.Models;

namespace MovieHub.ViewModels;

public class MovieRuntimeViewModel
{
    public IList<MovieRuntime> MovieRuntimes { get; set; }
    public List<Hall>? Halls { get; set; }
}