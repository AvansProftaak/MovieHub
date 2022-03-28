using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }
    
    [PersonalData]
    public string? LastName { get; set; }
    
}