using Booking.Domain.DbContexts;

namespace Booking.Domain.Seed
{
	public abstract class DataSeedBuilderBase
    {
        protected readonly BookingDbContext Context;
        protected bool shouldSave = false;

        public DataSeedBuilderBase(BookingDbContext context)
        {
            Context = context;
        }

        public void Seed()
        {
            Create();
            if (shouldSave)
            {
                Context.SaveChanges();
            }
        }

        protected abstract void Create();
    }
}
