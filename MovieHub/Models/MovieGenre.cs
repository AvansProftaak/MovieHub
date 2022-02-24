namespace MovieHub.Models;

public class MovieGenre
{
    public int Id { get; set; }
    public int GenreId { get; set; }
    public int MovieId { get; set; }
    
    public virtual Genre? Pegi { get; set;  }
    public virtual Movie? Movie { get; set;  }

    public MovieGenre()
    {
        
    }
}