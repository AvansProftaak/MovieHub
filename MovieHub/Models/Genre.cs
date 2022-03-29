using System.Collections.Generic;
using System.ComponentModel;

namespace MovieHub.Models;

public class Genre
{
    public int Id { get; set; }
    [DisplayName("Genre")]
    public GenreEnum GenreEnum { get; set; }
    
    public IList<MovieGenre>? MovieGenres { get; set; }

    public Genre()
    {
    }
}