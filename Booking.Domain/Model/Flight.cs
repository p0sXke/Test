using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Model
{
	public class Flight
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int DepartureId { get; set; }

		public int DestinationId { get; set; }

		public DateTime DateTime { get; set; }

		public int StopNumber { get; set; }

		public int SeatNumber { get; set; }

		public bool Canceled { get; set; } = false;
	}
}
