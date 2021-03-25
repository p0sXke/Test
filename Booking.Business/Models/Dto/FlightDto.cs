using System;
using System.ComponentModel.DataAnnotations;

namespace Booking.Business.Models.Dto
{
	public class FlightDto
	{
		public int Id { get; set; }

		[Required, Range(1, int.MaxValue)]
		public int DepartureId { get; set; }
		public string Departure { get; set; }

		[Required, Range(1, int.MaxValue)]
		public int DestinationId { get; set; }
		public string Destination { get; set; }

		[Required, Display(Name = "Date/Time")]
		public DateTime DateTime { get; set; }

		[Required, Range(0, int.MaxValue)]
		[Display(Name = "Stops")]
		public int StopNumber { get; set; }
		
		[Required, Range(0, int.MaxValue)]
		[Display(Name = "Seats")]
		public int SeatNumber { get; set; }

		public bool Canceled { get; set; }
	}
}
