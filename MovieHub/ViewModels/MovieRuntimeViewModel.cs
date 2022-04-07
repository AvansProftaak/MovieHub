using MovieHub.Models;

namespace MovieHub.ViewModels;

public class MovieRuntimeViewModel
{
    public IList<MovieRuntime> MovieRuntimes { get; set; }
    public IList<Hall> Halls { get; set; }
}