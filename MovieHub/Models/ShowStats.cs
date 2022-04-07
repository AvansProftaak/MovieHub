using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

[NotMapped]
public class ShowStats
{
    public DateTime Showtime { get; set; }   
    public string MovieTitle { get; set; }
    public string Hall { get; set; }
    public int SeatsFree { get; set; }

    public ShowStats()
    {
        
    }
}