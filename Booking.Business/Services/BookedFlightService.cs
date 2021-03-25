using AutoMapper;
using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services
{
	public class BookedFlightService : IBookedFlightService
	{
		private readonly IBookedFlightRepository _repository;
		private readonly IFlightRepository _flightRepository;
		private readonly IMapper _mapper;

		public BookedFlightService(IBookedFlightRepository repository, IFlightRepository flightRepository, IMapper mapper)
		{
			_repository = repository;
			_flightRepository = flightRepository;
			_mapper = mapper;
		}

		public async Task<BookedFlightDto> ChangeBookingStatus(int id, BookingStatusEnum bookingStatus)
		{
			var existing = await _repository.GetAsync(id);

			if (existing == null)
				throw new Exception("Booking with given id not found");

			var flight = await _flightRepository.GetAsync(existing.FlightId);

			switch (bookingStatus)
			{
				case BookingStatusEnum.Approved:
					if (existing.BookingStatusId != (int)BookingStatusEnum.Approved)
					{
						if (flight.SeatNumber - existing.BookSeats >= 0)
						{
							flight.SeatNumber -= existing.BookSeats;
							existing.BookingStatusId = (int)bookingStatus;

							await _flightRepository.UpdateAsync(flight);
							await _repository.UpdateAsync(existing);
						}
						else
							throw new Exception("Number of booked seat exceeds the limit");
					}
					break;
				case BookingStatusEnum.Rejected:
					if (existing.BookingStatusId == (int)BookingStatusEnum.Approved)
					{
						flight.SeatNumber += existing.BookSeats;
						await _flightRepository.UpdateAsync(flight);
					}
					existing.BookingStatusId = (int)bookingStatus;
					await _repository.UpdateAsync(existing);
					break;
				default:
					break;
			}

			return _mapper.Map<BookedFlightDto>(existing);
		}

		public async Task<BookedFlightDto> Create(int flightId, int seatNumber, int userId)
		{
			var existing = await _flightRepository.GetAsync(flightId);
			
			if (existing == null)
				throw new Exception("Flight with given id not found");

			if (!DateValidation(existing))
				throw new Exception("Booking must be 72 hours before the flight.");

			if (!SeatValidation(existing, seatNumber))
				throw new Exception("Number of booked seats exceeds free seats.");


			var newEntity = new BookedFlight() { BookingStatusId = (int)BookingStatusEnum.Submited, FlightId = flightId, BookSeats = seatNumber, UserProfileId = userId };
			var newEntityId = await _repository.CreateAsync(newEntity);

			var bookedFlight = await _repository.GetAsync(newEntityId);

			return _mapper.Map<BookedFlightDto>(bookedFlight);
		}

		public async Task Delete(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public async Task<BookedFlightDto> Get(int id)
		{
			return _mapper.Map<BookedFlightDto>(await _repository.GetAsync(id));
		}

		public async Task<List<BookedFlightDto>> GetAll()
		{
			return _mapper.Map<List<BookedFlightDto>>(await _repository.GetAllAsync());
		}
		
		public async Task<List<BookedFlightDto>> GetUsersBookedFlights(int userId)
		{
			return _mapper.Map<List<BookedFlightDto>>(await _repository.GetAllForUserAsync(userId));
		}

		public async Task<BookedFlightDto> Update(int id, int seatNumber)
		{
			var existing = await _repository.GetAsync(id);

			if (existing == null)
				throw new Exception("Booking with given id not found");

			if (!SeatValidation(existing.Flight, seatNumber))
				throw new Exception("Number of booked seats exceeds free seats.");

			existing.BookSeats = seatNumber;
			var newEntity = await _repository.UpdateAsync(existing);

			return _mapper.Map<BookedFlightDto>(newEntity);
		}

		private bool DateValidation(Flight existing)
		{
			if ((existing.DateTime - DateTime.UtcNow).TotalDays < 3)
				return false;

			return true;
		}
		
		private bool SeatValidation(Flight existing, int seatNumber)
		{
			if (existing.SeatNumber < seatNumber)
				return false;

			return true;
		}
	}
}
