using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Model
{
	public class UserProfile : IdentityUser<int>
	{
		[StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

		[StringLength(100)]
		public override string UserName { get; set; }

		[StringLength(100)]
		public override string NormalizedUserName { get; set; }

		public string Notification { get; set; }
    }
}
