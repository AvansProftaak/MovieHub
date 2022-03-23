using Microsoft.AspNetCore.Identity;

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
                Email = "admin@moviehub.nl"
            };
            var result = userManager.CreateAsync(user, "Welkom@01").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Manager").Wait();
            }
        }
    }

    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
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
        
        if (!roleManager.RoleExistsAsync("User").Result)
        {
            var role = new IdentityRole
            {
                Name = "User"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }  
    } 
    
}