// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IdentityAndAccess.DbMigrations;

internal class Program
{
    public static void Main(string[] args)
    {
        var host =  WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

        SeedData.EnsureSeedData(host.Services);

        // Exit the application
        Console.WriteLine("Exiting application...");
        Environment.Exit(0);
    }
}
