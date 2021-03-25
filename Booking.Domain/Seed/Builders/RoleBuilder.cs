using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Utils;
using System;
using System.Linq;

namespace Booking.Domain.Seed.Builders
{
	public class RoleBuilder : DataSeedBuilderBase, IDataSeedBuilder
    {
        public RoleBuilder(BookingDbContext context)
            : base(context)
        {
        }

        protected override void Create()
        {
            foreach (RoleEnum value in Enum.GetValues(typeof(RoleEnum)))
            {
                var existing = Context.Roles.FirstOrDefault(r => r.Id == (int)value);

                var shouldBe = new Role
                {
                    Id = (int)value,
                    Name = Enum.GetName(typeof(RoleEnum), value),
                    NormalizedName = Enum.GetName(typeof(RoleEnum), value).ToUpper(),
                    DisplayName = EnumHelper.GetDescription(value)
                };

                if (existing == null)
                {
                    Context.Roles.Add(shouldBe);
                    Context.SaveChanges();
                }
                else if (existing.Name != shouldBe.Name || existing.DisplayName != shouldBe.DisplayName)
                {
                    Context.Entry(existing).CurrentValues.SetValues(shouldBe);
                    Context.SaveChanges();
                }
            }

			var allForDeletion = Context.Roles.ToList().Where(t => !Enum.IsDefined(typeof(RoleEnum), t.Id));
			
			if (allForDeletion.Any())
            {
                Context.Roles.RemoveRange(allForDeletion);
                Context.SaveChanges();
            }
        }
    }
}
