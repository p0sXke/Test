using Booking.Business.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services.Interfaces
{
	public interface IFlightService
	{
        Task<List<FlightDto>> GetAll();

        Task<FlightDto> Get(int id);

        Task<int> Create(FlightDto flight);

        Task<FlightDto> Update(FlightDto flight);

        Task Delete(int id);
        
        Task CancelFlight(int id);
    }
}
