using AutoMapper;
using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Booking.Domain.Model;
using Booking.Domain.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services
{
	public class UserProfileService : IUserProfileService
	{
		private readonly IUserProfileRepository _repository;
		private readonly IMapper _mapper;

		public UserProfileService(IUserProfileRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<int> Create(UserProfileDto user)
		{
			return await _repository.CreateAsync(_mapper.Map<UserProfile>(user), user.RoleId);
		}

		public async Task Delete(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public async Task<UserProfileDto> Get(int id)
		{
			var user = await _repository.GetAsync(id);
			var userDto = _mapper.Map<UserProfileDto>(user);

			var role = (await _repository.GetRoleAsync(user));

			userDto.RoleId = role.Id;
			userDto.Role = role.Name;
			
			return userDto;
		}

		public async Task<List<UserProfileDto>> GetAll()
		{
			return _mapper.Map<List<UserProfileDto>>(await _repository.GetAllAsync());
		}

		public async Task<UserProfileDto> Update(UserProfileDto user)
		{
			var newEntity = await _repository.UpdateAsync(_mapper.Map<UserProfile>(user), user.RoleId);

			return _mapper.Map<UserProfileDto>(newEntity);
		}
	}
}
