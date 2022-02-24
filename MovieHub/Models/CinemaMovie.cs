namespace MovieHub.Models;

public class CinemaMovie
{
    public int Id { get; set; }
    public int CinemaId { get; set; }
    public int MovieId { get; set; }
    
    public virtual Cinema? Cinema { get; set;  }
    public virtual Movie? Movie { get; set;  }

    public CinemaMovie()
    {
        
    }
}