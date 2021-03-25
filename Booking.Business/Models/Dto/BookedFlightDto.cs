using System;
using System.ComponentModel.DataAnnotations;

namespace Booking.Business.Models.Dto
{
	public class BookedFlightDto
	{
		public int Id { get; set; }

		[Display(Name = "Booked seats")]
		public int BookSeats { get; set; }

		[Range(1, int.MaxValue)]
		public int FlightId { get; set; }
		
		[Display(Name = "Departure")]
		public string FlightDeparture { get; set; }

		[Display(Name = "Destination")]
		public string FlightDestination { get; set; }

		[Display(Name = "Date/Time")]
		public DateTime FlightDateTime { get; set; }

		[Display(Name = "Stops")]
		public int FlightStopNumber { get; set; }

		[Display(Name = "Seats")]
		public int FlightSeatNumber { get; set; }
		
		[Range(1, int.MaxValue)]
		public int BookingStatusId { get; set; }

		[Display(Name = "Booking status")]
		public string BookingStatus { get; set; }

		[Range(1, int.MaxValue)]
		public int UserProfileId { get; set; }
		public string UserProfileFirstName { get; set; }
		public string UserProfileLastName { get; set; }

		[Display(Name = "User")]
		public string UserProfileFullName => UserProfileFirstName + " " + UserProfileLastName;
		
		[Display(Name = "Email")]
		public string UserProfileEmail { get; set; }

	}
}
