// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.EntityFramework.Storage;
using IdentityAndAccess.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAndAccess.DbMigrations;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration config)
    {
        Configuration = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // var connectionString = Configuration.GetConnectionString("ProductionConnection");
        var identityServerConnectionString = Configuration.GetConnectionString("IdentityServerTestConnection");
        var aspnetIdentityConnectionString = Configuration.GetConnectionString("AspnetIdentityTestConnection");
        var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

        services.AddDbContext<AspIdentityDbContext>(dbContextOptionsBuilder =>
            dbContextOptionsBuilder.UseNpgsql(aspnetIdentityConnectionString,
                dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly)));

        services.AddOperationalDbContext(options =>
        {
            options.ConfigureDbContext = dbContextOptionsBuilder =>
                dbContextOptionsBuilder.UseNpgsql(identityServerConnectionString,
                    dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly));
        });

        services.AddConfigurationDbContext(options =>
        {
            options.ConfigureDbContext = dbContextOptionsBuilder =>
                dbContextOptionsBuilder.UseNpgsql(identityServerConnectionString,
                    dbOptions => dbOptions.MigrationsAssembly(migrationsAssembly));
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AspIdentityDbContext>()
            .AddDefaultTokenProviders();
    }

    public void Configure()
    {
    }
}
