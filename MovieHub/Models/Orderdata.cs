using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class OrderData
{
    public int movieId { get; set; }
    public Decimal TotalPrice { get; set; }
    
    [DisplayName( "User")] 
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [DisplayName( "Showtime")] 
    public int ShowtimeId { get; set; }
    [ForeignKey("ShowtimeId")]
    public virtual Showtime Showtime { get; set; } = null!;


    public OrderData()
    {
        
    }
}