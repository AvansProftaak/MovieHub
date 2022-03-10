using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class Payment
{
    public int Id { get; set; }
    public double Balance { get; set; }
    public DateTime? PaidAt { get; set; }
    public StatusEnum Status { get; set; }
    
    [DisplayName( "Order")] 
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    [DisplayName( "PaymentMethod")] 
    public int PaymentMethodId { get; set; }
    [ForeignKey("PaymentMethodId")]
    public PaymentMethod PaymentMethod { get; set; } = null!;

    public Payment()
    {
        
    }
    
}