using Booking.ConfigurationServices;
using Booking.Domain.DbContexts;
using Booking.Domain.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Booking
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

				var configuration = services.GetRequiredService<IConfigurationService>();
				try
                {
                    var context = services.GetRequiredService<BookingDbContext>();
					if (configuration.RunMigrationsOnStartup)
					{
						context.Database.Migrate();
					}

					if (configuration.RunSeedOnStartup)
					{
						SeedData.Initialize(services);
					}
				}
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred updating the DB.");
                }
            }

            host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
