using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace MovieHub.Models;

public class Order
{
    public int Id { get; set; }
    public double TotalPrice { get; set; }
    public DateTime PaidAt { get; set; }
    public enum Status
    {
        Open = 1,
        Pending = 2,
        Paid = 3
    }
    
    [DisplayName( "User")] 
    public int ShowtimeId { get; set; }
    [ForeignKey("UserId")]
    public virtual Showtime Showtime { get; set; }
    
    [DisplayName( "Showtime")] 
    public int UserId { get; set; }
    [ForeignKey("ShowtimeId")]
    public virtual User User { get; set; }
    
    [DisplayName( "CateringPackage")] 
    public int CateringPackageId { get; set; }
    [ForeignKey("CateringPackageId")]
    public virtual CateringPackage CateringPackage { get; set; }
    public Order()
    {
        
    }
}