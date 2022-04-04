using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class ListRoleViewModel
{
    public ListRoleViewModel(User user, List<IdentityRole> rolesNotAdded, List<IdentityRole> rolesAdded, UserManager<User> userManager)
    {
        User = user;
        RolesNotAdded = rolesNotAdded;
        RolesAdded = rolesAdded;
        UserManager = userManager;
    }

    public User? User { get;set; }
    public List<IdentityRole>? RolesNotAdded { get; set; }
    public List<IdentityRole>? RolesAdded { get; set; } 
    public UserManager<User>? UserManager { get; set; }

}