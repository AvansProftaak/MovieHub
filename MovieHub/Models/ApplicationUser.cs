using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace MovieHub.Models;

public class ApplicationUser : IdentityUser
{
    [Required] 
    public string Name { get; set; }
}