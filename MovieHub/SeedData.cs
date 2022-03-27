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
        if (userManager.FindByEmailAsync("admin@moviehub.nl").Result != null) return;
        var user = new IdentityUser
        {
            UserName = "Admin",
            Email = "admin@moviehub.nl",
            EmailConfirmed = true
        };
        var result = userManager.CreateAsync(user, "Admin1!").Result;
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(user, "Admin").Wait();
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

        if (roleManager.RoleExistsAsync("User").Result) return;
        {
            var role = new IdentityRole
            {
                Name = "User"
            };
            var result  = roleManager.CreateAsync(role).Result;
        }
    } 
    
}