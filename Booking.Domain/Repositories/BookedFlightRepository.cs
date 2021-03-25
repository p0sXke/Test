using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories
{
	public class BookedFlightRepository : IBookedFlightRepository
	{
		private readonly BookingDbContext _context;

		public BookedFlightRepository(BookingDbContext context)
		{
			_context = context;
		}

		public async Task<int> CreateAsync(BookedFlight entity)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var result = await _context.BookedFlights.AddAsync(entity);
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
					var existing = await _context.BookedFlights.FirstOrDefaultAsync(x => x.Id == entityId) ?? throw new Exception("Booked flight with given identifier not found.");
					_context.BookedFlights.Remove(existing);
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

		public async Task<List<BookedFlight>> GetAllAsync()
		{
			return await _context.BookedFlights
				.AsNoTracking()
				.Include(x => x.BookingStatus)
				.Include(x => x.Flight)
				.Include(x => x.UserProfile)
				.ToListAsync();
		}
		
		public async Task<List<BookedFlight>> GetAllForUserAsync(int userId)
		{
			return await _context.BookedFlights
				.AsNoTracking()
				.Include(x => x.BookingStatus)
				.Include(x => x.Flight)
				.Include(x => x.UserProfile)
				.Where(x => x.UserProfileId == userId)
				.ToListAsync();
		}

		public async Task<BookedFlight> GetAsync(int entityId)
		{
			var existing = await _context.BookedFlights
				.AsNoTracking()
				.Include(x => x.BookingStatus)
				.Include(x => x.Flight)
				.Include(x => x.UserProfile)
				.FirstOrDefaultAsync(x => x.Id == entityId) ??

				throw new Exception("Booked flight with given identifier not found.");

			return existing;
		}

		public async Task<BookedFlight> UpdateAsync(BookedFlight entity)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var existing = await _context.BookedFlights.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? throw new Exception("Booked flight with given identifier not found.");
					_context.Entry(existing).CurrentValues.SetValues(entity);
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue updating item in the Database." + e.Message, e);
				}
			}

			return await _context.BookedFlights
				.AsNoTracking()
				.Include(x => x.BookingStatus)
				.Include(x => x.Flight)
				.Include(x => x.UserProfile)
				.FirstOrDefaultAsync(x => x.Id == entity.Id);
		}
	}
}
