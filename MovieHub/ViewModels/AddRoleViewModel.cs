using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieHub.ViewModels;

public class AddRoleViewModel
{
    [Required]
    [Display(Name = "Role")]
    public string? SelectedRole { get; set; }
    public string? SelectedUserId { get; set; }
    
    public IEnumerable<string>? AvailableRoles { get; set; }
    public IEnumerable<IdentityUser>? Users { get; set; }
}