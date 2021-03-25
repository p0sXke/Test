using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories
{
	public class FlightRepository : IFlightRepository
	{
		private readonly BookingDbContext _context;

		public FlightRepository(BookingDbContext context)
		{
			_context = context;
		}

		public async Task<int> CreateAsync(Flight entity)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var result = await _context.Flights.AddAsync(entity);
					await _context.SaveChangesAsync();

					transaction.Commit();

					return result.Entity.Id;
				}
				catch (DbUpdateException e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue adding to Database", e);
				}
			}
		}

		public async Task DeleteAsync(int entityId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var existing = await _context.Flights.FirstOrDefaultAsync(x => x.Id == entityId) ?? throw new Exception("Flight with given identifier not found.");
					_context.Flights.Remove(existing);
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue deleting item from the Database." + e.Message, e);
				}
			}
		}

		public async Task<List<Flight>> GetAllAsync()
		{
			return await _context.Flights.AsNoTracking().ToListAsync();
		}

		public async Task<Flight> GetAsync(int entityId)
		{
			var existing = await _context.Flights
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == entityId) ??
				
				throw new Exception("Flight with given identifier not found."); 

			return existing;
		}

		public async Task<Flight> UpdateAsync(Flight entity)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var existing = await _context.Flights.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? throw new Exception("Flight with given identifier not found.");
					_context.Entry(existing).CurrentValues.SetValues(entity);
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue deleting item from the Database." + e.Message, e);
				}
			}

			return await _context.Flights.FirstOrDefaultAsync(x => x.Id == entity.Id);
		}
	}
}
