using System.Text.Json;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SchoolDonations.App.Server.Configuration;

namespace SchoolDonations.App.Server.Pages.Auth;

public class AuthorizationCodeFlow2Model : PageModel
{
    public string CallResult { get; private set; }

    private IdentityServerSettings IdentityServerSettings { get; }
    private ApiSettings ApiSettings { get; }

    private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

    public AuthorizationCodeFlow2Model(
        IOptionsSnapshot<IdentityServerSettings> identityServerSettings,
        IOptionsSnapshot<ApiSettings> apiSettings)
    {
        IdentityServerSettings = identityServerSettings.Value;
        ApiSettings = apiSettings.Value;
    }

    public async Task OnGet()
    {
        // Use automatic access token, refresh token lifetime handling.
        var tokenInfo = await HttpContext.GetUserAccessTokenAsync();
        var client = new HttpClient();
        client.SetBearerToken(tokenInfo.AccessToken!);

        var response = await client.GetAsync($"{ApiSettings.BaseUrl}/heartbeat/identity");
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            var parsed = JsonDocument.Parse(data);
            var formatted = JsonSerializer.Serialize(parsed, CachedJsonSerializerOptions);

            CallResult = formatted;
        }
    }
}