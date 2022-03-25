using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public bool AcceptedNewsletter { get; set; }
}