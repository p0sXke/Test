using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Utils;
using System;
using System.Linq;

namespace Booking.Domain.Seed.Builders
{
	public class CitiesBuilder : DataSeedBuilderBase, IDataSeedBuilder
    {
        public CitiesBuilder(BookingDbContext context)
            : base(context)
        {
        }

        protected override void Create()
        {
            foreach (CityEnum value in Enum.GetValues(typeof(CityEnum)))
            {
                var existing = Context.Cities.FirstOrDefault(r => r.Id == (int)value);

                var shouldBe = new City
                {
                    Id = (int)value,
                    Name = Enum.GetName(typeof(CityEnum), value),
                    DisplayName = EnumHelper.GetDescription(value)
                };

                if (existing == null)
                {
                    Context.Cities.Add(shouldBe);
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

            var allForDeletion = Context.Cities.ToList().Where(t => !Enum.IsDefined(typeof(CityEnum), t.Id));
			if (allForDeletion.Any())
            {
                Context.Cities.RemoveRange(allForDeletion);
                Context.SaveChanges();
            }
        }
    }
}
