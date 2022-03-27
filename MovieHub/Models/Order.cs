using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class Order
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    
    [DisplayName( "User")] 
    public string? UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [DisplayName( "Showtime")] 
    public int ShowtimeId { get; set; }
    [ForeignKey("ShowtimeId")]
    public virtual Showtime? Showtime { get; set; }

}
