using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public bool AcceptedNewsletter { get; set; }

    public User()
    {
        
    }
}