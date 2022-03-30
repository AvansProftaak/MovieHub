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

    public BackOfficeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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

    public async Task<IActionResult> AddRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var result = await _userManager.AddToRoleAsync(user, role);
        
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