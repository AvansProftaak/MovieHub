using System.ComponentModel.DataAnnotations.Schema;
namespace MovieHub.Models;

public class Showtime
{
    public int Id { get; set; }
    public DateTime StartAt { get; set; }
    public int HallId { get; set; }
    public int MovieId { get; set; }
    
    [ForeignKey("HallId")]
    public Hall Hall { get; set; } = null!;
    
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }

    public Showtime()
    {
        
    }

}