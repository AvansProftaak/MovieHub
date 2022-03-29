using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub;

public static class SeedData
{
    public static void Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        SeedRoles(roleManager);
        SeedUsers(userManager);
    }

    private static void SeedUsers(UserManager<IdentityUser> userManager)
    {
        if (userManager.FindByNameAsync("admin").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@moviehub.com"
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, RoleEnum.Administrator.ToString()).Wait();
            }
        }
        
        if (userManager.FindByNameAsync("employee").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "employee",
                Email = "employee@moviehub.com"
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, RoleEnum.Employee.ToString()).Wait();
            }
        }
        
        if (userManager.FindByNameAsync("backofficeemployee").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "backofficeemployee",
                Email = "backofficeemployee@moviehub.com"
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, RoleEnum.BackOfficeEmployee.ToString()).Wait();
            }
        }
    }
    
    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.RoleExistsAsync(RoleEnum.Administrator.ToString()).Result)
        {
            var role = new IdentityRole
            {
                Name = RoleEnum.Administrator.ToString()
            };
            var result = roleManager.CreateAsync(role).Result;
        }
        
        if (!roleManager.RoleExistsAsync(RoleEnum.Employee.ToString()).Result)
        {
            var role = new IdentityRole
            {
                Name = RoleEnum.Employee.ToString()
            };
            var result = roleManager.CreateAsync(role).Result;
        }
        
        if (!roleManager.RoleExistsAsync(RoleEnum.BackOfficeEmployee.ToString()).Result)
        {
            var role = new IdentityRole
            {
                Name = RoleEnum.BackOfficeEmployee.ToString()
            };
            var result = roleManager.CreateAsync(role).Result;
        }
    }
}