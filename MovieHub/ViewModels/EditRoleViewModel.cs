using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class EditRoleViewModel
{
    public EditRoleViewModel(User? user, IdentityRole roleToChange)
    {
        User = user;
        RoleToChange = roleToChange;
    }

    public User? User { get;set; }
    public IdentityRole? RoleToChange { get; set; }
    
}
    
    