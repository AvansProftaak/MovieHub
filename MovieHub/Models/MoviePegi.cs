namespace MovieHub.Models;

public class MoviePegi
{
    public int Id { get; set; }
    public int PegiId { get; set; }
    public int MovieId { get; set; }
    
    public virtual Pegi? Pegi { get; set;  }
    public virtual Movie? Movie { get; set;  }

    public MoviePegi()
    {
        
    }

}