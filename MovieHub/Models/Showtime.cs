namespace MovieHub.Models;

public class Showtime
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public int MovieId { get; set; }
    public DateTime StartAt { get; set; }

    public Showtime()
    {
        
    }
    
}