using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    
    [HttpGet]
    public IActionResult RoleList()
    {
        var roles = _roleManager.Roles;
        
        return View(roles);
    }

    [HttpGet]
    public IActionResult AddRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
            return View("Error");
        }

        var model = new EditRoleViewModel
        {
            Id = role.Id,
            RoleName = role.Name
        };

        foreach (var user in _userManager.Users.ToList())
        {
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                model.Users.Add(user.UserName);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.Id);
        
        if (role == null)
        {
            ViewData["ErrorMessage"] = $"No role with Id '{model.Id}' was found";
            return View("Error");
        }

        else
        {
            role.Name = model.RoleName;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditUsersInRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        ViewData["roleId"] = id;
        ViewData["roleName"] = role.Name;

        if (role == null)
        {
            ViewData["ErrorMessage"] = "No role with Id '{id}' was found";
            return View("Error");
        }

        var model = new List<UserRoleViewModel>();

        foreach (var user in _userManager.Users.ToList())
        {
            var userRoleViewModel = new UserRoleViewModel()
            {
                Id = user.Id,
                Name = user.UserName
            };

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRoleViewModel.IsSelected = true;
            }
            else
            {
                userRoleViewModel.IsSelected = false;
            }
            
            model.Add(userRoleViewModel);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        
        if (role == null)
        {
            ViewData["ErrorMessage"] = "No role with Id '{id}' was found";
            return View("Error");
        }

        for (int i = 0; i < model.Count; i++)
        {
            var user = await _userManager.FindByIdAsync(model[i].Id);

            if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            else
            {
                continue;
            }
        }

        return RedirectToAction("EditRole", new {Id = id});
    }
}