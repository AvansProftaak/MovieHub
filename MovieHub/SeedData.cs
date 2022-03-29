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
                Email = "admin@moviehub.com",
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Administrator").Wait();
            }
        }
        
        if (userManager.FindByNameAsync("employee").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "employee",
                Email = "employee@moviehub.com",
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Employee").Wait();
            }
        }
        
        if (userManager.FindByNameAsync("backofficeemployee").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "backofficeemployee",
                Email = "backofficeemployee@moviehub.com",
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, "password").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Backoffice Employee").Wait();
            }
        }
    }
    
    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.RoleExistsAsync("Administrator").Result)
        {
            var role = new IdentityRole
            {
                Name = "Administrator"
            };
            var result = roleManager.CreateAsync(role).Result;
        }
        
        if (!roleManager.RoleExistsAsync("Employee").Result)
        {
            var role = new IdentityRole
            {
                Name = "Employee"
            };
            var result = roleManager.CreateAsync(role).Result;
        }
        
        if (!roleManager.RoleExistsAsync("Backoffice Employee").Result)
        {
            var role = new IdentityRole
            {
                Name = "Backoffice Employee"
            };
            var result = roleManager.CreateAsync(role).Result;
        }
    }
}