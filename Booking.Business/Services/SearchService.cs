using AutoMapper;
using Booking.Business.Models.Dto;
using Booking.Business.Models.Request;
using Booking.Business.Services.Interfaces;
using Booking.Domain.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Business.Services
{
	public class SearchService : ISearchService
	{
		private readonly BookingDbContext _context;
		private readonly IMapper _mapper;

		public SearchService(BookingDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<FlightDto>> GetFlightsSearchResult(FlightSearchRequest searchRequest)
		{
			var result = await _context.Flights
				.AsNoTracking()
				.Where(x => x.DepartureId == searchRequest.DepartureId && x.DestinationId == searchRequest.DestinationId && x.SeatNumber > 0 && (searchRequest.NoStops ? x.StopNumber == 0 : x.StopNumber >= 0))
				.OrderBy(x => x.DateTime)
				.ToListAsync();

			return _mapper.Map<List<FlightDto>>(result);
		}
	}
}
