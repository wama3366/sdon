using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolDonations.App.Server.Pages.Auth;

public class OidcIdentity : PageModel
{
    public Dictionary<string, string> Items { get; set; } = [];

    public async Task OnGet()
    {

        var authenticateResult = await HttpContext.AuthenticateAsync();
        if (authenticateResult.Succeeded)
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var idToken = await HttpContext.GetTokenAsync("id_token");
            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //Items = authenticateResult.Properties!.Items.ToDictionary();
        }
    }
}