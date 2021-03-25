using Booking.Domain.DbContexts;
using Booking.Domain.Seed.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Booking.Domain.Seed
{
	public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookingDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookingDbContext>>()))
            {
                Initialize(context);
            }
        }

        public static void Initialize(BookingDbContext context)
        {
            new BookingStatusBuilder(context).Seed();
            new CitiesBuilder(context).Seed();
            new RoleBuilder(context).Seed();
            new UserProfileBuilder(context).Seed();
        }
    }
}
