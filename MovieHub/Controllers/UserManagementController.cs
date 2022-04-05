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
    public readonly UserManager<User?> _userManager;
    public readonly ApplicationDbContext _context;
    
    
    public UserManagementController(RoleManager<IdentityRole> roleManager, UserManager<User?> userManager, ApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }


    // GET
    public IActionResult Index()
    {
        
        return View(GetRoles());
    }
    
    public async Task<IActionResult> Edit(string roleId)
    {
        var taskRole = GetRole(roleId);
        var role = taskRole.Result;
        var users = GetUsers();
        
        List<User> usersAdded = new List<User>();
        List<User> usersNotAdded = new List<User>();
        
        foreach (User user in users)
        {
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                usersAdded.Add(user);
            }
            else
            {
                usersNotAdded.Add(user);
            }
        }

        List<EditRoleViewModel> editModel = new List<EditRoleViewModel>();
        foreach (User user in usersAdded)
        {
            string status = "added";
            EditRoleViewModel viewModel= new EditRoleViewModel(
                user,
                role,
                status,
                _userManager,
                _context,
                _roleManager);
            editModel.Add(viewModel);
        }
        
        foreach (User user in usersNotAdded)
        {
            string status = "not added";
            EditRoleViewModel viewModel= new EditRoleViewModel(
                user,
                role,
                status,
                _userManager,
                _context,
                _roleManager);
            editModel.Add(viewModel);
        }
        
        ListRoleViewModel model = new ListRoleViewModel(role, editModel);
        
        return View(model);
    }

    private async Task<List<IdentityRole>> getNotAddedRoles(User? user, List<IdentityRole> allRoles)
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

    private async Task<List<IdentityRole>> getAddedRoles(User? user, List<IdentityRole> allRoles)
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

    public ICollection<User> GetUsers()
    {
        return _context.Users.ToList();
    }
    
    public ICollection<IdentityRole> GetRoles()
    {
        return _context.Roles.ToList();
    }

    public Task<User?> GetUser(string userId)
    {
        
        var user = _userManager.FindByIdAsync(userId);
        return user;
    }

    public static async Task RemoveRole(EditRoleViewModel model, UserManager<User> userManager,ApplicationDbContext context)
    {
         var result = userManager.RemoveFromRoleAsync(model.User, model.RoleToChange.NormalizedName);
         await context.SaveChangesAsync();

    }

    public static async Task AddRole(EditRoleViewModel model, UserManager<User> userManager,ApplicationDbContext context)
    {
        var result = userManager.AddToRoleAsync(model.User, model.RoleToChange.NormalizedName);
        await context.SaveChangesAsync();
        
    }

    public IActionResult RoleChanged(Task<string> userid)
    {
        return View();
    }
    
    public async Task ChangeRole( string userId, string roleId, string status)
    {
        User user = await GetUser(userId);
        IdentityRole role = await GetRole(roleId);
        
        if (status == "added")
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.NormalizedName);
        }
        
        if (status == "not added")
        {

            var result = await _userManager.AddToRoleAsync(user, role.NormalizedName);
        }

        //await _context.SaveChangesAsync();
    }

    public async Task<IdentityResult> AddRole(EditRoleViewModel model)
    {
       var userToChange = await GetUser(model.User.Id);
        var roleToChange = await GetRole(model.RoleToChange.Id);


        return await _userManager.AddToRoleAsync(userToChange, roleToChange.NormalizedName);
        //await _userManager.SaveChangesAsync();

    }

    private async Task<IdentityRole> GetRole(string roleId)
    {
        return  await _roleManager.FindByIdAsync(roleId);
        
    }
}