using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData] public string? FirstName { get; set; } = null!;

    [PersonalData] public string? LastName { get; set; } = null!;

    [PersonalData] public bool? AcceptedNewsletter { get; set; } = null!;
}