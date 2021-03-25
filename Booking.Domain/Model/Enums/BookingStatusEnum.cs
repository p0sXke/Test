using System.ComponentModel;

namespace Booking.Domain.Model.Enums
{
	public enum BookingStatusEnum
	{
		[Description("Submited")]
		Submited = 1,

		[Description("Approved")]
		Approved,

		[Description("Rejected")]
		Rejected
	}
}
