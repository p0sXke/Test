using Booking.Business.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Business.Services.Interfaces
{
	public interface IUserProfileService
	{

        Task<List<UserProfileDto>> GetAll();

        Task<UserProfileDto> Get(int id);

        Task<int> Create(UserProfileDto user);

        Task<UserProfileDto> Update(UserProfileDto user);

        Task Delete(int id);
	}
}
