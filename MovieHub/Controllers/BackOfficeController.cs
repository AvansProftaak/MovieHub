using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Data;

namespace MovieHub.Controllers;

[Authorize]
public class BackOfficeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BackOfficeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult ManageUsers()
    {
        return View( _context.Users.ToList());
    }

    public async void RemoveRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        _userManager.RemoveFromRoleAsync(user, role).Wait();
        ManageUsers();
    }
}