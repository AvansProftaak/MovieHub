using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class AddRoleViewModel
{
    public AddRoleViewModel(User user, List<IdentityRole> rolesNotAdded, List<IdentityRole> rolesAdded)
    {
        User = user;
        RolesNotAdded = rolesNotAdded;
        RolesAdded = rolesAdded;
    }

    public User? User { get;set; }
    public List<IdentityRole>? RolesNotAdded { get; set; }
    public List<IdentityRole>? RolesAdded { get; set; }
    
}