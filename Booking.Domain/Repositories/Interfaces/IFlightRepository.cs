using Booking.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories.Interfaces
{
	public interface IFlightRepository
	{
        Task<int> CreateAsync(Flight entity);

        Task<List<Flight>> GetAllAsync();

        Task<Flight> GetAsync(int entityId);

        Task DeleteAsync(int entityId);

        Task<Flight> UpdateAsync(Flight entity);
    }
}
