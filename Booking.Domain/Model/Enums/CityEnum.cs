using System.ComponentModel;

namespace Booking.Domain.Model.Enums
{
	public enum CityEnum
	{
        [Description("Belgrade")]
        Beograd = 1,

        [Description("Nis")]
        Nis = 2,

        [Description("Kraljevo")]
        Kraljevo = 3,

        [Description("Pristina")]
        Pristina = 4
    }
}
