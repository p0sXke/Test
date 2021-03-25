using System.ComponentModel.DataAnnotations;

namespace Booking.Business.Models.Dto
{
	public class LoginDto
	{
		[Required(AllowEmptyStrings = false), StringLength(20, MinimumLength = 3)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false), StringLength(20, MinimumLength = 3)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Password { get; set; }
	}
}
