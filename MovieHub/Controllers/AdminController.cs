using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

[Authorize(Roles = "Admin, Manager")]
public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    public IActionResult RoleList()
    {
        var roles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
        
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
                model.Users.Add(user);
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
                FirstName = user.FirstName,
                LastName = user.LastName,
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

        var admins = 0;

        if (role.Name == "Admin")
        {
            foreach (var usr in model)
            {
                if (usr.IsSelected)
                {
                    admins =+ 1;
                }
            }
        }


        foreach (var usr in model)
        {
            var user = await _userManager.FindByIdAsync(usr.Id);

            if (usr.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            else if ((role.Name != "Admin" || admins > 0) && !usr.IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            else if ((role.Name == "Admin" || admins == 0) && !usr.IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
            {
                return RedirectToAction("EditRole", new {id = role.Id});
            }
            else
            {
                continue;
            }
        }

        return RedirectToAction("EditRole", new {Id = id});
    }

    [HttpGet]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        return View(role);

    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            ViewData["ErrorMessage"] = "No role with Id '{id}' was found";
            return View("Error");
        }
        else
        {
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("RoleList");
        }
    }
    
}
