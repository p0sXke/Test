using System.ComponentModel.DataAnnotations;

namespace Booking.Business.Models.Dto
{
	public class UserProfileDto
	{
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }
        
        [Required, StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        
        [Required, StringLength(20, MinimumLength = 5)]
        public string Email { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Notification { get; set; }

        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}
