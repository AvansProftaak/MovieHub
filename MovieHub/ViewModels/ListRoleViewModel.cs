using Microsoft.AspNetCore.Identity;
using MovieHub.Data;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class ListRoleViewModel
{
    public ListRoleViewModel(IdentityRole role, List<EditRoleViewModel> editRoleViewModel)
    {
        Role = role;
        EditRoleViewModel = editRoleViewModel;
    }
    
    public IdentityRole Role { get; set; }
    public List<EditRoleViewModel>? EditRoleViewModel { get; set; } 

}