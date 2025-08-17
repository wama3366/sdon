using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SchoolDonations.App.Server.Configuration;

namespace SchoolDonations.App.Server.Pages.Auth;

public class AuthorizationCodeFlow3Model : PageModel
{
    public string CallResult { get; private set; }

    private IdentityServerSettings IdentityServerSettings { get; }
    private ApiSettings ApiSettings { get; }
    private IHttpClientFactory HttpClientFactory { get; }

    private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

    public AuthorizationCodeFlow3Model(
        IOptionsSnapshot<IdentityServerSettings> identityServerSettings,
        IOptionsSnapshot<ApiSettings> apiSettings,
        IHttpClientFactory httpClientFactory)
    {
        IdentityServerSettings = identityServerSettings.Value;
        ApiSettings = apiSettings.Value;
        HttpClientFactory = httpClientFactory;
    }

    public async Task OnGet()
    {
        // Use IdentityServer HttpClient factory to get an HttpClient that automatically handles
        // access tokens and refresh tokens.
        // This re-uses the http client as needed instead of creating a new one like in AuthorizationCodeFlow2Model
        var client = HttpClientFactory.CreateClient("IdentityServerHttpClient");
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