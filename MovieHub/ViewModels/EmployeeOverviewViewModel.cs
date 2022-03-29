using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieHub.Models;

namespace MovieHub.ViewModels;

public class EmployeeOverviewViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public IEnumerable<string> Roles { get; set; }
}