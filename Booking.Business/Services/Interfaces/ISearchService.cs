using Booking.Business.Models.Dto;
using Booking.Business.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services.Interfaces
{
	public interface ISearchService
	{
		Task<List<FlightDto>> GetFlightsSearchResult(FlightSearchRequest searchRequest);
	}
}
