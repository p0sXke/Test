using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Utils;
using System;
using System.Linq;

namespace Booking.Domain.Seed.Builders
{
    public class BookingStatusBuilder : DataSeedBuilderBase, IDataSeedBuilder
    {
        public BookingStatusBuilder(BookingDbContext context)
            : base(context)
        {
        }

        protected override void Create()
        {
            foreach (BookingStatusEnum value in Enum.GetValues(typeof(BookingStatusEnum)))
            {
                var existing = Context.BookingStatuses.FirstOrDefault(r => r.Id == (int)value);

                var shouldBe = new BookingStatus
                {
                    Id = (int)value,
                    Name = Enum.GetName(typeof(BookingStatusEnum), value),
                    DisplayName = EnumHelper.GetDescription(value)
                };

                if (existing == null)
                {
                    Context.BookingStatuses.Add(shouldBe);
                    Context.SaveChanges();
                }
                else
                {
                    if (existing.Name != shouldBe.Name || existing.DisplayName != shouldBe.DisplayName)
                    {
                        Context.Entry(existing).CurrentValues.SetValues(shouldBe);
                        Context.SaveChanges();
                    }
                }
            }

            var allForDeletion = Context.BookingStatuses.ToList().Where(t => !Enum.IsDefined(typeof(BookingStatusEnum), t.Id));
            if (allForDeletion.Any())
            {
                Context.BookingStatuses.RemoveRange(allForDeletion);
                Context.SaveChanges();
            }
        }
    }
}