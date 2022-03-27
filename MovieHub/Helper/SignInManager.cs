using Microsoft.AspNetCore.Identity;
using MovieHub.Models;

namespace MovieHub.Helper;

public class MovieHubSignInManager : SignInManager<ApplicationUser>
{
    public MovieHubSignInManager(
        UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor contextAccessor, 
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, 
        Microsoft.Extensions.Options.IOptions<IdentityOptions> optionsAccessor, 
        ILogger<SignInManager<ApplicationUser>> logger, 
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation
        )
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password,
        bool isPersistent, bool lockoutOnFailure)
    {
        var user = await UserManager.FindByEmailAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }
}