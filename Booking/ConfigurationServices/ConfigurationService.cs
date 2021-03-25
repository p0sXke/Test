using Microsoft.Extensions.Configuration;

namespace Booking.ConfigurationServices
{
	public class ConfigurationService : IConfigurationService
	{
		private readonly IConfiguration _configuration;

		public ConfigurationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string DefaultConnection => _configuration["ConnectionStrings:DefaultConnection"];

		public bool RunMigrationsOnStartup => string.IsNullOrWhiteSpace(_configuration["RunMigrationsOnStartup"]) ? true : bool.Parse(_configuration["RunMigrationsOnStartup"]);

		public bool RunSeedOnStartup => string.IsNullOrWhiteSpace(_configuration["RunSeedOnStartup"]) ? true : bool.Parse(_configuration["RunSeedOnStartup"]);
	}
}
