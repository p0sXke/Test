using Booking.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Domain.DbContexts
{
	public class BookingDbContext : IdentityDbContext<UserProfile, Role, int>
	{
		public virtual DbSet<BookingStatus> BookingStatuses { get; set; }
		public virtual DbSet<BookedFlight> BookedFlights { get; set; }
		public virtual DbSet<City> Cities { get; set; }
		public virtual DbSet<Flight> Flights { get; set; }
		public override DbSet<Role> Roles { get; set; }
		public virtual DbSet<UserProfile> UserProfiles { get; set; }

		public BookingDbContext()
		{
		}

		public BookingDbContext(DbContextOptions<BookingDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<BookedFlight>()
			   .HasOne(x => x.Flight)
			   .WithMany()
			   .HasForeignKey(x => x.FlightId);

			modelBuilder.Entity<BookedFlight>()
				.HasOne(x => x.UserProfile)
				.WithMany()
				.HasForeignKey(x => x.UserProfileId);
		}
	}
}
