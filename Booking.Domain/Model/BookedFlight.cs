using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Model
{
	public class BookedFlight
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }

		public int BookSeats { get; set; }

		public virtual Flight Flight { get; set; }
		public int FlightId { get; set; }

		public virtual BookingStatus BookingStatus { get; set; }
		public int BookingStatusId { get; set; }

		public virtual UserProfile UserProfile { get; set; }
		public int UserProfileId { get; set; }
	}
}
