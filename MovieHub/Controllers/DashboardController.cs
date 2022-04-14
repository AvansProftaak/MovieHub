using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Data;
using MovieHub.ViewModels;
using System.Net.Mail;
using System.Net;

namespace MovieHub.Controllers
{
    [Authorize(Roles = "Admin, BackOffice, Cashier")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(ILogger<DashboardController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var rolesUser = await _userManager.GetRolesAsync(user);
            if(rolesUser.Count == 1 && rolesUser.Contains("BackOffice"))
            {
                return RedirectToAction("Index", "CashierDashboard");
            } 
            else if (rolesUser.Count == 1 && rolesUser.Contains("Cashier"))
            {
                return RedirectToAction("Index", "CashierDashboard");
            } else
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            AdminViewModel adminViewModel = new AdminViewModel()
            {
                roles = _roleManager.Roles.Select(x => x.Name).ToList(),
                users = _userManager.Users.Select(x => x.UserName).ToList()
            };
            return View(adminViewModel);
        }

        public async Task<IActionResult> InsertUser(string username, string password)
        {
            var user = CreateUser();
            user.UserName = username;
            user.EmailConfirmed = true;
            user.Email = username;
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminDashboard");
            } else
            {
                return RedirectToAction("AdminDashboard");
            }
        }

        public async Task<IActionResult> AddRoleToUser(string[] rolesWithUsers)
        {
            foreach (var roleAndUser in rolesWithUsers)
            {
                string[] splittedRoleAndUser = roleAndUser.Split("-");
                string role = splittedRoleAndUser[0];
                var user = await _userManager.FindByEmailAsync(splittedRoleAndUser[1]);
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> InsertRole(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
            return RedirectToAction("AdminDashboard");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
