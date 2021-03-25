using System.ComponentModel.DataAnnotations;

namespace Booking.Business.Models.Request
{
	public class FlightSearchRequest
	{
		[Range(1, int.MaxValue)]
		public int DepartureId	{get; set;}

		[Range(1, int.MaxValue)]
		public int DestinationId { get; set; }

		public bool NoStops { get; set; } = false;
	}
}
