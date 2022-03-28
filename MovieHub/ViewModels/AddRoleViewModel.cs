using System.ComponentModel.DataAnnotations;

namespace MovieHub.ViewModels;

public class AddRoleViewModel
{
    [Required]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; }
}