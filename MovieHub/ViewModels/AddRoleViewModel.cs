using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class AddRoleViewModel
{
    public User? User { get;set; }
    public List<IdentityRole> Roles { get; set; }
    public List<IdentityRole> RolesAdded { get; set; }
    
}