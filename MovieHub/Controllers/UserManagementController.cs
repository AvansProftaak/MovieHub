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
    
    public readonly RoleManager<IdentityRole> _roleManager;
    public readonly UserManager<User> _userManager;
    public readonly ApplicationDbContext _context;
    
    
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

        ListRoleViewModel model = new ListRoleViewModel(
            user,
            rolesNotAdded,
            rolesAdded,
            _userManager
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

    public async void RemoveRole(EditRoleViewModel model, UserManager<User> userManager)
    {
         var result = userManager.RemoveFromRoleAsync(model.User, model.RoleToChange.Name);

    }

    public static void AddRole(EditRoleViewModel model, UserManager<User> userManager)
    {
        var result = userManager.AddToRoleAsync(model.User, model.RoleToChange.Name);
        
    }
}