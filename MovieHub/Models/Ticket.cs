using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Ticket
{
    public int Id { get; set; }
    public int Barcode { get; set; }
    public double Price { get; set; }
    public StatusEnum Status { get; set; }
    
    public enum StatusEnum
    {
        Unpaid,
        Paid
    }

    [DisplayName( "Order")] 
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [DisplayName( "Tickettype")] 
    public int TickettypeId { get; set; }
    [ForeignKey("TickettypeId")]
    public virtual Tickettype Tickettype { get; set; } = null!;
    
    [DisplayName( "Seat")] 
    public int SeatId { get; set; }
    [ForeignKey("SeatId")]
    public virtual Seat Seat { get; set; } = null!;
    
    public Ticket()
    {
        
    }
}