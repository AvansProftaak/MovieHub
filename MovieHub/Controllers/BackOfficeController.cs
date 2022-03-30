using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Data;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

[Authorize]
public class BackOfficeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AddRoleViewModel> _logger;

    public BackOfficeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager, ILogger<AddRoleViewModel> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult ManageUsers()
    {
        var allRoles = _roleManager.Roles.ToList();
        var roles = new List<String>();
        foreach (var role in allRoles)
        {
            roles.Add(role.Name);
        }

        var vm = new AddRoleViewModel
        {
            Users = _context.Users.ToList(),
            AvailableRoles = roles
        };

        return View(vm);
    }

    public async Task<IActionResult> RemoveRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        
        if (result.Succeeded)
        {
            return RedirectToAction("ManageUsers");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ManageUsers");
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        _logger.LogInformation(model.SelectedRole);
        var user = await _userManager.FindByIdAsync(model.SelectedUserId);
        var result = await _userManager.AddToRoleAsync(user, model.SelectedRole);
        
        if (result.Succeeded)
        {
            return RedirectToAction("ManageUsers");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ManageUsers");
    }
}