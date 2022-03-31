using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub;

public static class SeedData
{
    public static void Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        SeedRoles(roleManager);
        SeedUsers(userManager);
    }

    private static void SeedUsers(UserManager<User> userManager)
    {
        if (userManager.FindByEmailAsync("admin@moviehub.nl").Result == null)
        {
            var user = new User
            {
                UserName = "admin@moviehub.nl",
                Email = "admin@moviehub.nl",
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, "Welkom@01").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }

    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        
        if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            var role = new IdentityRole
            {
                Name = "Admin"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }  
        
        if (!roleManager.RoleExistsAsync("Manager").Result)
        {
            var role = new IdentityRole
            {
                Name = "Manager"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }   
        
        if (!roleManager.RoleExistsAsync("Back-Office").Result)
        {
            var role = new IdentityRole
            {
                Name = "Back-Office"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }   
        
        if (!roleManager.RoleExistsAsync("Front-Office").Result)
        {
            var role = new IdentityRole
            {
                Name = "Front-Office"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }   

    } 
    
}