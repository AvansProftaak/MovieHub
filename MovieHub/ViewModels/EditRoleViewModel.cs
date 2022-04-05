using Microsoft.AspNetCore.Identity;
using MovieHub.Data;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class EditRoleViewModel
{
    
    public User? User { get;set; }
    public IdentityRole? RoleToChange { get; set; }
    public String? Status { get; set; }
    public UserManager<User?> _UserManager { get; set; }
    public RoleManager<IdentityRole> _RoleManager { get; set; }
    public ApplicationDbContext _Context { get; set; }

    public EditRoleViewModel(User? user, IdentityRole roleToChange, string status, UserManager<User?> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        User = user;
        RoleToChange = roleToChange;
        Status = status;
        _UserManager = userManager;
        _Context = context;
        _RoleManager = roleManager;
    }

    public async Task<bool> EditRole()
    {
        if (Status == "added")
        {
            var result = await _UserManager.RemoveFromRoleAsync(User, RoleToChange.NormalizedName);
            
        }
        
        if (Status == "not added")
        {
            var result = await _UserManager.AddToRoleAsync(User, RoleToChange.NormalizedName);
        }
        return true;
    }
    
}
    
    