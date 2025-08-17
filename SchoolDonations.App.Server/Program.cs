using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SchoolDonations.App.Server.Configuration;
using Serilog;
using System.Globalization;
using Yarp.ReverseProxy.Configuration;

namespace SchoolDonations.App.Server;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting up.");

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            services.AddRazorPages();
            services.AddControllers();

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    formatProvider: CultureInfo.InvariantCulture)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ctx.Configuration));

            services.Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptionsSnapshot<IdentityServerSettings>>().Value);

            services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptionsSnapshot<ApiSettings>>().Value);

            ConfigureAuth(builder);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseBff();
            app.UseAuthorization();
            app.MapBffManagementEndpoints();
            app.MapStaticAssets();
            app.MapControllers()
                .RequireAuthorization()
                .AsBffApiEndpoint();
            app.MapRazorPages()
                .WithStaticAssets()
                .RequireAuthorization();

            var apiSettings = builder.Configuration
                .GetSection("ApiSettings")
                .Get<ApiSettings>();

            app.MapRemoteBffApiEndpoint("/remote", apiSettings.BaseUrl)
                .RequireAccessToken();

            app.MapFallbackToFile("/index.html");

            app.MapBffReverseProxy();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            Log.Information("Shutting down.");
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureAuth(WebApplicationBuilder builder)
    {
        var identityServerSettings = builder.Configuration
            .GetSection("IdentityServerSettings")
            .Get<IdentityServerSettings>();

        var services = builder.Services;
        services.AddBff()
            .AddRemoteApis();

        // Adds the flow handlers for different authentication schemes.
        // We only allow oidc and cookies here.
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
            // After the user log in at the Identity Server and gets redirected
            // back here, a cookie will be set to maintain the session between the browser and the website only to indicate
            // the user has already signed in. This doesn't affect the tokens issued by Identity Server and can expire independently
            // from them.
            .AddCookie("Cookies", options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;  // or Strict
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            // Changes here may require changes to how Identity Server is set up.
            // (Matching URLs, IDs, Secrets, etc...)

            // Add Oidc Client
            .AddOpenIdConnect(authenticationScheme: "oidc", options =>
            {
                // Identity Server URL
                options.Authority = identityServerSettings.Authority;
                options.ClientId = "OidcClient";
                options.ClientSecret = "secret";
                // Authorization Code flow.
                options.ResponseType = OpenIdConnectResponseType.Code;

                // Use those to request access to resources. They need to be accepted/matched by IdentityServer.
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("SchoolDonations.Api.Read");
                options.Scope.Add("SchoolDonations.Api.Write");
                // Allows requesting a refresh token.
                options.Scope.Add("offline_access");

                // After authenticating a user, reach out to the userinfo endpoint to retrieve additional user info.
                options.GetClaimsFromUserInfoEndpoint = true;

                // Do not map claims to .NET-style claims. Leave them as they are.
                options.MapInboundClaims = false;

                // Saves the access token, refresh token and id token in the cookie to be used later.
                options.SaveTokens = true;
            });

        // Automatically handles refresh tokens etc...
        services.AddOpenIdConnectAccessTokenManagement();

        services.AddReverseProxy()
            .LoadFromMemory(
                // Routes
                [
                    new RouteConfig
                    {
                        RouteId   = "account-proxy",
                        ClusterId = "idsrv-account",
                        Match     = new RouteMatch { Path = "/account/{**catch-all}" }
                    }
                ],
                // Clusters
                [
                    new ClusterConfig
                    {
                        ClusterId = "idsrv-account",
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            ["dest1"] = new DestinationConfig
                            {
                                // Note the trailing slash ensures PathAndQuery is preserved
                                Address = $"{identityServerSettings.Authority}/account/"
                            }
                        }
                    }
                ])
            // this wires in cookie-to-header transforms, anti-forgery checks, etc.
            .AddBffExtensions();

        // Builds on top of HttpClientFactory and creates an HttpClient that automatically refreshes tokens as needed.
        services.AddUserAccessTokenHttpClient("IdentityServerHttpClient",
            configureClient: client => { client.BaseAddress = new Uri(identityServerSettings.Authority); });
    }
}