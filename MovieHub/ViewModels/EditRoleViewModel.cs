using System.ComponentModel.DataAnnotations;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class EditRoleViewModel
{
    public string Id { get; set; }
    
    [Display(Name = "Role")]
    [Required(ErrorMessage = "Role name is required")]
    public string RoleName { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();
}