using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class MovieGenre
{
    public int Id { get; set; }

    [DisplayName( "Genre")] 
    public int GenreId { get; set; }
    [ForeignKey("GenreId")]
    public virtual Genre? Genre { get; set;  }

    [DisplayName( "Movie")] 
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie? Movie { get; set;  }

}