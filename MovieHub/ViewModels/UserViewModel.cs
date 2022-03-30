using Microsoft.AspNetCore.Identity;

namespace MovieHub.ViewModels;

public class UserViewModel
{
    public IdentityUser? IdentityUser { get; set; }
    public List<string> Roles { get; set; }
}