using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SchoolDonations.UserInterface.Configuration;

namespace SchoolDonations.UserInterface.Pages.Auth
{
    public class AuthorizationCodeFlowModel : PageModel
    {
        public string CallResult { get; private set; }

        private IdentityServerSettings IdentityServerSettings { get; }
        private ApiSettings ApiSettings { get; }

        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

        public AuthorizationCodeFlowModel(
            IOptionsSnapshot<IdentityServerSettings> identityServerSettings,
            IOptionsSnapshot<ApiSettings> apiSettings)
        {
            IdentityServerSettings = identityServerSettings.Value;
            ApiSettings = apiSettings.Value;
        }

        public async Task OnGet()
        {
            // Use manual access token, refresh token lifetime handling.
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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
}