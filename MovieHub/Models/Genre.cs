using System.ComponentModel;

namespace MovieHub.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public IList<MovieGenre>? MovieGenres { get; set; }

}