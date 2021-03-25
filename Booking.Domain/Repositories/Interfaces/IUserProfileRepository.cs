using Booking.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories.Interfaces
{
	public interface IUserProfileRepository
	{

        Task<int> CreateAsync(UserProfile entity, int roleId);

        Task<List<UserProfile>> GetAllAsync();

        Task<UserProfile> GetAsync(int entityId);
        
        Task<Role> GetRoleAsync(UserProfile entity);

        Task DeleteAsync(int entityId);

        Task<UserProfile> UpdateAsync(UserProfile entity, int roleId);
    }
}
