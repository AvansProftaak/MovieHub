using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

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
    
    public async Task<IActionResult> Edit(string userId)
    {
        var taskUser = GetUSer(userId);
        User user = taskUser.Result;
        List<IdentityRole> allRoles = _roleManager.Roles.ToList();
        List<IdentityRole> rolesAdded = await getAddedRoles(user, allRoles);
        List<IdentityRole> rolesNotAdded = await getNotAddedRoles(user, allRoles);

        AddRoleViewModel model = new AddRoleViewModel(
            user,
            rolesNotAdded,
            rolesAdded
            );
        
        return View(model);
    }

    private async Task<List<IdentityRole>> getNotAddedRoles(User user, List<IdentityRole> allRoles)
    {
        var notAddedRoles = new List<IdentityRole>();
        
        foreach (IdentityRole role in allRoles)
        {
            var check = await _userManager.IsInRoleAsync(user, role.Name);
            if (!check)
            {
                notAddedRoles.Add(role);
            }
        }

        return notAddedRoles;
    }

    private async Task<List<IdentityRole>> getAddedRoles(User user, List<IdentityRole> allRoles)
    {
        var addedRoles = new List<IdentityRole>();
        
        foreach (IdentityRole role in allRoles)
        {
            var check = await _userManager.IsInRoleAsync(user, role.Name);
            if (check)
            {
                addedRoles.Add(role);
            }
        }

        return addedRoles;
    }

    public ICollection<User> GetUSers()
    {
        return _context.Users.ToList();
    }

    public Task<User> GetUSer(string userId)
    {
        
        var user = _userManager.FindByIdAsync(userId);
        return user;
    }

    public async Task<Task<IdentityResult>> RemoveRole(User user, string name)
    {
        return _userManager.RemoveFromRoleAsync(user, name);
    }

    public async Task<Task<IdentityResult>> AddRole(User user, string name)
    {
        return _userManager.AddToRoleAsync(user, name);
    }
}