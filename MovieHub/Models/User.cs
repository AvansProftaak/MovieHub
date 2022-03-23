using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public bool AcceptedNewsletter { get; set; }

    public User()
    {
        
    }
}