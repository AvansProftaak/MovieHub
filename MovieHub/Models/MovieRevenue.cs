using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

[NotMapped]
public class MovieRevenue
{
    public string MovieTitle { get; set; } = null!;
    public int AmountShows { get; set; }
    public string TicketRevenue { get; set; } = null!;
    public string ArrangementRevenue { get; set; } = null!; 
    public string TotalRevenue { get; set; } = null!;

    public MovieRevenue()
    {
        
    }
}