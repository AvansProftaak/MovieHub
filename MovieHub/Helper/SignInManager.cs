using Microsoft.AspNetCore.Identity;

namespace MovieHub.Helper;

public class MovieHubSignInManager : SignInManager<IdentityUser>
{
    public MovieHubSignInManager(
        UserManager<IdentityUser> userManager, 
        IHttpContextAccessor contextAccessor, 
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, 
        Microsoft.Extensions.Options.IOptions<IdentityOptions> optionsAccessor, 
        ILogger<SignInManager<IdentityUser>> logger, 
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation
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