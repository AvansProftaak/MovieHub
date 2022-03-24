using System.ComponentModel.DataAnnotations;

namespace MovieHub.ViewModels;

public class AddRoleViewmodel
{
    [Required]
    [Display(Name = "Role")]
    public string RoleName  { get; set; }
}