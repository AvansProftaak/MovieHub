using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public bool? AcceptedNewsletter { get; set; } = null!;
}