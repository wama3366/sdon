
using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Test;
using IdentityAndAccess.API.Middleware;
using IdentityAndAccess.API.Models;
using IdentityAndAccess.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Concepts;
using Utilities.AppDateTime;
using Serilog;

namespace IdentityAndAccess.API;

public class Program
{
    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting up.");

        try
        {
            const string uiRoot = "UI";
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                WebRootPath = $"{uiRoot}/wwwroot"
            });

            builder.Services.AddRazorPages(options => { options.RootDirectory = $"/{uiRoot}/Pages"; });

            builder.Services.AddAuthentication();
            ConfigureSerilog(builder);
            ConfigureDb(builder);
            ConfigureIdentityAndAccess(builder);
            ConfigureDi(builder);

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages().RequireAuthorization();

            #region Exception Handling

            app.UseMiddleware<ExceptionHandler>();

            #endregion Exception Handling

            app.Run();
        }
        catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
        {
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            Log.Information("Shutting down.");
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureIdentityAndAccess(WebApplicationBuilder builder)
    {
        #region Aspnet Identity

        var aspnetIdentityDbSettings = builder.Configuration
            .GetSection("AspnetIdentityDbSettings")
            .Get<DbSettings>();

        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        builder.Services.AddDbContext<AspIdentityDbContext>(dbContextOptionsBuilder =>
            dbContextOptionsBuilder.UseNpgsql(aspnetIdentityDbSettings.ConnectionString,
                dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly)));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AspIdentityDbContext>()
            .AddDefaultTokenProviders();

        #endregion Aspnet Identity

        #region  Identity Server

        var identityServerBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.UserInteraction.LoginUrl = "/Account/Login";
            options.UserInteraction.LogoutUrl = "/Account/Logout";
        });

        var identityServerDbSettings = builder.Configuration
            .GetSection("IdentityServerDbSettings")
            .Get<DbSettings>();

        identityServerBuilder
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = dbContextOptionsBuilder =>
                    dbContextOptionsBuilder.UseNpgsql(identityServerDbSettings.ConnectionString,
                        dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly));
            })
            // this is something you will want in production to reduce load on and requests to the DB
            //.AddConfigurationStoreCache()
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = dbContextOptionsBuilder =>
                    dbContextOptionsBuilder.UseNpgsql(identityServerDbSettings.ConnectionString,
                        dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly));

                // Override the default table name since Keys is a reserved keyword in the dbms.
                options.Keys.Name = "SigningKeys";
                options.EnableTokenCleanup = true;
            })
            .AddServerSideSessions()
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>();

        if (builder.Environment.IsDevelopment())
        {
            identityServerBuilder
                //.AddTestUsers(TestUsers)
                .AddDeveloperSigningCredential();
        }

        #endregion Identity Server
    }

    private static List<TestUser> TestUsers =>
    [
        new()
        {
            SubjectId = "1",
            Username = "wassim",
            Password = "wassim"
        }
    ];


    private static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                formatProvider: CultureInfo.InvariantCulture)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(ctx.Configuration));
    }

    private static void ConfigureDb(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptionsSnapshot<DbSettings>>().Value);

        services.AddDbContext<ConfigurationDbContext>();
        services.AddDbContext<PersistedGrantDbContext>();
    }

    private static void ConfigureDi(WebApplicationBuilder builder)
    {
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterType<AppDateTime>()
                .As<IAppDateTime>()
                .SingleInstance();
        });
    }
}