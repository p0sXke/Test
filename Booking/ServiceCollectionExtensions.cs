using AutoMapper;
using Booking.Business.Models.Mapping;
using Booking.Business.Services;
using Booking.Business.Services.Interfaces;
using Booking.ConfigurationServices;
using Booking.Domain.Repositories;
using Booking.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Booking
{
	public static class ServiceCollectionExtensions
	{
		public static void AddApplicationServices(this IServiceCollection services)
		{
			//services.AddSingleton<ISystemClockService, SystemClockService>();
			services.AddSingleton<IConfigurationService, ConfigurationService>();
			services.AddScoped<IBookedFlightService, BookedFlightService>();
			services.AddScoped<IFlightService, FlightService>();
			services.AddScoped<ISearchService, SearchService>();
			services.AddScoped<IUserProfileService, UserProfileService>();
		}

		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IBookedFlightRepository, BookedFlightRepository>();
			services.AddScoped<IFlightRepository, FlightRepository>();
			services.AddScoped<IUserProfileRepository, UserProfileRepository>();
		}

		public static void AddAutoMapper(this IServiceCollection services)
		{
			var profile = new MappingProfile();
			var configuration = new MapperConfiguration(c => c.AddProfile(profile));
			var mapper = configuration.CreateMapper();
			services.AddSingleton(mapper);
		}

	}
}
