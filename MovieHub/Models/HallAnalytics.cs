using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

[NotMapped]
public class HallAnalytics
{
    public string HallName { get; set; } = null!;
    public string MovieTitle { get; set; } = null!;
    public DateTime Showtime { get; set; }
    public int HallCapacity { get; set; }
    public int SeatsTaken { get; set; }
    public int SeatsFree { get; set; }

    public HallAnalytics()
    {
        
    }
}