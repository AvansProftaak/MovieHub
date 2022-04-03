using System.ComponentModel;
using Microsoft.Build.Framework;

namespace MovieHub.Models;

public class Voucher
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int Movies { get; set; }
    public bool paid { get; set; }
    public string Barcode { get; set; }
    [DisplayName( "First Name")] 
    public string FirstName { get; set; }
    [DisplayName( "Last Name")] 
    public string LastName { get; set; }
    public Voucher()
    {
        
    }
}