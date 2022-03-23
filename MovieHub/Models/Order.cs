using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class Order
{
    public int Id { get; set; }
    public Decimal TotalPrice { get; set; }
    
    [DisplayName( "User")] 
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual IdentityUser? User { get; set; } = null!;

    [DisplayName( "Showtime")] 
    public int ShowtimeId { get; set; }
    [ForeignKey("ShowtimeId")]
    public virtual Showtime? Showtime { get; set; } = null!;


    public Order()
    {
        
    }
}
