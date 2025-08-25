using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDD.Concepts.BaseTypes;
using Microsoft.Extensions.Options;
using Persistence.Concepts;
using SchoolDonations.API.Configuration;
using SchoolDonations.API.Controllers.Customers;
using SchoolDonations.API.Middleware;
using SchoolDonations.ApplicationServices;
using SchoolDonations.ApplicationServices.Services.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;
using SchoolDonations.EFCore;
using SchoolDonations.EFCore.Customers;
using SchoolDonations.EFCore.DomainEvents;
using Serilog;
using Utilities.AppDateTime;

namespace SchoolDonations.API;

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
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    formatProvider: CultureInfo.InvariantCulture)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ctx.Configuration));

            builder.Services.AddControllers();
            ConfigureAuth(builder);
            ConfigureDb(builder);
            ConfigureDi(builder);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers()
                .RequireAuthorization();
            app.UseMiddleware<ExceptionHandler>();

            app.Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Unhandled exception");
            throw;
        }
        finally
        {
            Log.Information("Shutting down.");
            Log.CloseAndFlush();
        }
	}

    private static void ConfigureDi(WebApplicationBuilder builder)
    {
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterType<AppDateTime>().As<IAppDateTime>().InstancePerLifetimeScope();

            #region Customers

            containerBuilder.RegisterType<CustomerPersistenceMapper>()
                .As<IPersistenceMapper<Customer, CustomerPersistenceDto>>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<CustomerService>().As<ICustomerService>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope();

            #endregion Customers

            #region DomainEvents

            containerBuilder.RegisterType<DomainEventPersistenceMapper>()
                    .As<IPersistenceMapper<DomainEvent, DomainEventDto>>()
                    .InstancePerLifetimeScope();

            containerBuilder.RegisterType<DomainEventRepository>().As<IDomainEventRepository>()
                .InstancePerLifetimeScope();

            #endregion DomainEvents

            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        });
    }

    private static void ConfigureDb(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

        // TODO: This will prevent the config from reading changes after each request.
        services.AddSingleton(sp => sp.GetRequiredService<IOptionsSnapshot<DbSettings>>().Value);

        // Defaults to ServiceLifetime.Scoped
        services.AddDbContext<AppDbContext>();
    }

    private static void ConfigureAuth(WebApplicationBuilder builder)
    {
        var identityServerSettings = builder.Configuration
            .GetSection("IdentityServerSettings")
            .Get<IdentityServerSettings>();
        var services = builder.Services;

        // Adds the flow handlers for different authentication schemes.
        // We only allow JWT bearer tokens here.
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = identityServerSettings.Authority;
                options.TokenValidationParameters.ValidateAudience = false;
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("SchoolDonations.Api.Read", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "SchoolDonations.Api.Read");
            })
            .AddPolicy("SchoolDonations.Api.Write", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "SchoolDonations.Api.Write");

            });
    }
}
