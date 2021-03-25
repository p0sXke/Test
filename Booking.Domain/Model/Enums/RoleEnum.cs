using System.ComponentModel;

namespace Booking.Domain.Model.Enums
{
	public enum RoleEnum
    {
        [Description("User")]
        User = 1,

        [Description("Agent")]
        Agent = 2,

        [Description("Administrator")]
        Admin = 3
    }
}
