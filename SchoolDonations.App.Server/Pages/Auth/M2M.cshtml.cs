using System.Text.Json;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SchoolDonations.App.Server.Configuration;

namespace SchoolDonations.App.Server.Pages.Auth;

public class M2MModel : PageModel
{
    public string CallResult { get; private set; }

    private IdentityServerSettings IdentityServerSettings { get; }
    private ApiSettings ApiSettings { get; }

    private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

    public M2MModel(
        IOptionsSnapshot<IdentityServerSettings> identityServerSettings,
        IOptionsSnapshot<ApiSettings> apiSettings)
    {
        IdentityServerSettings = identityServerSettings.Value;
        ApiSettings = apiSettings.Value;
    }

    public async Task OnGet()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(IdentityServerSettings.Authority);
        if (disco.IsError)
        {
            return;
        }

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "M2M",
            ClientSecret = "secret",
            Scope = "SchoolDonations.Api.Read SchoolDonations.Api.Write"
        });

        if (tokenResponse.IsError)
        {
            return;
        }

        var apiClient = new HttpClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken!);

        var response = await apiClient.GetAsync($"{ApiSettings.BaseUrl}/heartbeat/identity");
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            var parsed = JsonDocument.Parse(data);
            var formatted = JsonSerializer.Serialize(parsed, CachedJsonSerializerOptions);

            CallResult = formatted;
        }
    }
}