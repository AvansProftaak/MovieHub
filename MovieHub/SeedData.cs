using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub;

public static class SeedData
{
    public static void Seed(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
    {
        SeedRoles(roleManager);
        SeedUsers(userManager);
    }
    
    private static void SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (userManager.FindByEmailAsync("admin@moviehub.com").Result == null)
        {
            var user = new ApplicationUser
            { 
                UserName = "Admin",
                Email = "admin@moviehub.com",
                FirstName = "Daniel",
                LastName= "Jansen",
                Street = "Fresiastraat",
                HouseNumber = 60,
                ZipCode = "4921HD",	
                City = "Made"
            };
            var result = userManager.CreateAsync(user, "P@ssword1").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Administrator").Wait();
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
        
        if (!roleManager.RoleExistsAsync("User").Result)
        {
            var role = new IdentityRole
            {
                Name = "User"
            };
            var result = roleManager.CreateAsync(role).Result;
        }        
    }
}