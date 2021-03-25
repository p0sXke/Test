using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Domain.Model
{
	public class Role : IdentityRole<int>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		//[Key]
		public override int Id { get; set; }

		[StringLength(100)]
		public override string Name { get; set; }

		[StringLength(100)]

		public override string NormalizedName {get;set;}

		[StringLength(100)]
        public string DisplayName { get; set; }
    }
}
