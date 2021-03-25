using Booking.Business.Models.Dto;
using Booking.Domain.Model.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services.Interfaces
{
	public interface IBookedFlightService
	{
		Task<List<BookedFlightDto>> GetAll();
		
		Task<List<BookedFlightDto>> GetUsersBookedFlights(int userId);

		Task<BookedFlightDto> Get(int id);

		Task<BookedFlightDto> Create(int flightId, int seatNumber, int userId);

		Task<BookedFlightDto> Update(int id, int bookSeats);

		Task Delete(int id);

		Task<BookedFlightDto> ChangeBookingStatus(int id, BookingStatusEnum bookingStatus);
	}
}
