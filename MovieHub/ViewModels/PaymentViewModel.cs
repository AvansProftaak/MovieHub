using Microsoft.AspNetCore.Mvc.Rendering;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class PaymentViewModel
{
    public Order Order { get; set; }
    public int PaymentMethodId { get; set; }
    public string? VoucherId { get; set; }
}