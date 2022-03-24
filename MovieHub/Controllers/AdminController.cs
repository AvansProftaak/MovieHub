using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
 
    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
    }

    // GET
    [HttpGet]
    public IActionResult ListAllRoles()
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
    public async Task<IActionResult> AddRole(AddRoleViewmodel model)
    {
        if (ModelState.IsValid)
        {
            IdentityRole identityRole = new()
            {
                Name = model.RoleName
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return RedirectToAction("ListAllRoles");
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
        var role = await _roleManager.FindByNameAsync(id);

        if (role == null)
        {
            ViewData["ErrorMessage"] = $"No role with Id {id} was found";
            return View("Errors");
        }

        EditRoleViewModel model = new()
        {
            Id = role.Id,
            RoleName = role.Name
        };
        foreach (var user in _userManager.Users)
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
            ViewData["ErrorMessage"] = $"No role with Id {model.Id} was found";
            return View("Errors");
        }
        else
        {
            role.Name = model.RoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListAllRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
     
}