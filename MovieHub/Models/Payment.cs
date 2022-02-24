using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Payment
{
    public int Id { get; set; }
    public double Balance { get; set; }
    public DateTime PaidAt { get; set; }

    public StatusEnum Status { get; set; }
    public enum StatusEnum 
    {
        Open,
        Pending,
        Paid
    }
    
    [DisplayName( "Order")] 
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [DisplayName( "PaymentMethod")] 
    public int PaymentMethodId { get; set; }
    [ForeignKey("PaymentMethodId")]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public Payment()
    {
        
    }
    
}