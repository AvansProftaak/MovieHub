using Microsoft.AspNetCore.Identity;
using MovieHub.Data;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class ListRoleViewModel
{
    public ListRoleViewModel(User user, List<IdentityRole> rolesNotAdded, List<IdentityRole> rolesAdded, UserManager<User> userManager, ApplicationDbContext context)
    {
        User = user;
        RolesNotAdded = rolesNotAdded;
        RolesAdded = rolesAdded;
        UserManager = userManager;
        Context = context;
    }

    public User? User { get;set; }
    public List<IdentityRole>? RolesNotAdded { get; set; }
    public List<IdentityRole>? RolesAdded { get; set; } 
    public UserManager<User>? UserManager { get; set; }
    public ApplicationDbContext? Context { get; set; }

}