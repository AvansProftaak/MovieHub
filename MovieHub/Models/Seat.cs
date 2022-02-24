namespace MovieHub.Models;

public class Seat
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public bool Available { get; set; }

    public Seat()
    {
        
    }
    
}