using AutoMapper;
using Booking.Business.Models.Dto;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Utils;

namespace Booking.Business.Models.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<BookedFlight, BookedFlightDto>()
				.ForMember(x => x.FlightDeparture, opt => opt.MapFrom(src => EnumHelper.GetDescription((CityEnum)src.Flight.DepartureId)))
				.ForMember(x => x.FlightDestination, opt => opt.MapFrom(src => EnumHelper.GetDescription((CityEnum)src.Flight.DestinationId)))
				.ForMember(x => x.FlightDateTime, opt => opt.MapFrom(src => src.Flight.DateTime))
				.ForMember(x => x.FlightStopNumber, opt => opt.MapFrom(src => src.Flight.StopNumber))
				.ForMember(x => x.FlightSeatNumber, opt => opt.MapFrom(src => src.Flight.SeatNumber))
				.ForMember(x => x.UserProfileFirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
				.ForMember(x => x.UserProfileLastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
				.ForMember(x => x.UserProfileEmail, opt => opt.MapFrom(src => src.UserProfile.Email))
				.ForMember(x => x.BookingStatus, opt => opt.MapFrom(src => EnumHelper.GetDescription((BookingStatusEnum)src.BookingStatus.Id)));
			CreateMap<BookedFlightDto, BookedFlight>(); 

			CreateMap<Flight, FlightDto>()
			   .ForMember(x => x.Departure, opt => opt.MapFrom(src => EnumHelper.GetDescription((CityEnum)src.DepartureId)))
			   .ForMember(x => x.Destination, opt => opt.MapFrom(src => EnumHelper.GetDescription((CityEnum)src.DestinationId)));
			CreateMap<FlightDto, Flight>();

			CreateMap<UserProfile, UserProfileDto>();
			CreateMap<UserProfileDto, UserProfile>()
				.ForMember(x => x.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()));
		}
	}
}
