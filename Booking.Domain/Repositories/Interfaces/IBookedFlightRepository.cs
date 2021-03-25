using Booking.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories.Interfaces
{
	public interface IBookedFlightRepository
	{
		Task<int> CreateAsync(BookedFlight entity);

		Task<List<BookedFlight>> GetAllAsync();

		Task<List<BookedFlight>> GetAllForUserAsync(int userId);

		Task<BookedFlight> GetAsync(int entityId);

		Task DeleteAsync(int entityId);

		Task<BookedFlight> UpdateAsync(BookedFlight entity);
	}
}
