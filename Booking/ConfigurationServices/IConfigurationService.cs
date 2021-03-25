namespace Booking.ConfigurationServices
{
	public interface IConfigurationService
	{
		string DefaultConnection { get; }

		bool RunMigrationsOnStartup { get; }

		bool RunSeedOnStartup { get; }
	}
}
