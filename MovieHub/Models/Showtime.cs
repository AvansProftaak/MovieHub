using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Showtime
{
    public int Id { get; set; }
    public DateTime StartAt { get; set; }

    [DisplayName( "Hall")] 
    public int HallId { get; set; }
    [ForeignKey("HallId")]
    public virtual Hall Hall { get; set; } = null!;

    [DisplayName( "Movie")] 
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; } = null!;

    public Showtime()
    {
        
    }
    
}