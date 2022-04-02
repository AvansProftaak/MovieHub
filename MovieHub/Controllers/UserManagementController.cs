using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;

namespace MovieHub.Controllers;

[Authorize (Roles = "Admin")]
public class UserManagementController : Controller
{
    
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    
    
    public UserManagementController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }
    
    
    // GET
    public IActionResult Index()
    {
        
        return View(GetUSers());
    }
    
    public IActionResult Edit(string userId)
    {
        var taskUser = GetUSer(userId);
        User? user = taskUser.Result;
        
        return View(user);
    }

    public ICollection<User> GetUSers()
    {
        return _context.Users.ToList();
    }

    public Task<User?> GetUSer(string userId)
    {
        
        var user = _context.User
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }
}