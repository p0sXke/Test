using AutoMapper;
using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Booking.Domain.Model;
using Booking.Domain.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services
{
	public class FlightService : IFlightService
	{
		private readonly IFlightRepository _repository;
		private readonly IMapper _mapper;

		public FlightService(IFlightRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task CancelFlight(int id)
		{
			Flight flight = await _repository.GetAsync(id);
			
			flight.Canceled = true;

			await _repository.UpdateAsync(flight);
		}

		public async Task<int> Create(FlightDto flight)
		{
			return await _repository.CreateAsync(_mapper.Map<Flight>(flight));
		}

		public async Task Delete(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public async Task<FlightDto> Get(int id)
		{
			var flight = _mapper.Map<FlightDto>(await _repository.GetAsync(id));

			return flight;
		}

		public async Task<List<FlightDto>> GetAll()
		{
			return _mapper.Map<List<FlightDto>>(await _repository.GetAllAsync());
		}

		public async Task<FlightDto> Update(FlightDto flight)
		{
			var newEntity = await _repository.UpdateAsync(_mapper.Map<Flight>(flight));

			return _mapper.Map<FlightDto>(newEntity);
		}
	}
}
