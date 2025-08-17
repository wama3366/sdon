using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IdentityAndAccess.API.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityAndAccess.API.Services;

public class ProfileService : ProfileService<ApplicationUser>
{
    public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
    {
    }

    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
    {
        var principal = await GetUserClaimsAsync(user);
        context.AddRequestedClaims(principal.Claims);
    }
}