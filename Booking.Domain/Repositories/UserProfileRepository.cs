using Booking.Domain.DbContexts;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Booking.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Domain.Repositories
{
	public class UserProfileRepository : IUserProfileRepository
	{
		private const string defaultPassword = "user";
		private readonly BookingDbContext _context;
		private readonly UserManager<UserProfile> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public UserProfileRepository(BookingDbContext context, UserManager<UserProfile> userManager, RoleManager<Role> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<int> CreateAsync(UserProfile entity, int roleId)
		{
			//entity.NormalizedUserName = entity.UserName.ToUpper();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var result = await _context.UserProfiles.AddAsync(entity);
					await _context.SaveChangesAsync();

					var user = await _userManager.FindByNameAsync(entity.UserName);
					if (user == null)
						user = _userManager.Users.FirstOrDefault(x => x.Email == entity.Email);

					if (user != null)
					{
						if (user.PasswordHash == null)
						{
							await _userManager.AddPasswordAsync(user, defaultPassword);
							
							await _context.SaveChangesAsync();
							transaction.Commit();

							await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), roleId));
						}
					}

					return result.Entity.Id;
				}
				catch (DbUpdateException e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue adding to Database", e);
				}
			}
		}

		public async Task DeleteAsync(int entityId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var existing = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == entityId) ?? throw new Exception("User with given identifier not found.");
					_context.UserProfiles.Remove(existing);
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue deleting item from the Database." + e.Message, e);
				}
			}
		}

		public async Task<List<UserProfile>> GetAllAsync()
		{
			return await _context.UserProfiles.AsNoTracking().ToListAsync();
		}

		public async Task<UserProfile> GetAsync(int entityId)
		{
			var existing = await _context.UserProfiles
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == entityId) ??

				throw new Exception("User with given identifier not found.");

			return existing;
		}

		public async Task<Role> GetRoleAsync(UserProfile entity)
		{
			Role role = new Role();

			var user = await _userManager.FindByNameAsync(entity.UserName);
			if (user == null)
				user = _userManager.Users.FirstOrDefault(x => x.Email == entity.Email);

			if (user != null)
			{
				string roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

				role = await _roleManager.FindByNameAsync(roleName);
			}

			return role;
		}

		public async Task<UserProfile> UpdateAsync(UserProfile entity, int roleId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var user = await _userManager.FindByNameAsync(entity.UserName);
					if (user == null)
						user = _userManager.Users.FirstOrDefault(x => x.Email == entity.Email);

					if (user == null)
						throw new Exception("User with given identifier not found.");
					else
					{
						var oldRoles = await _userManager.GetRolesAsync(user);
						await _userManager.RemoveFromRolesAsync(user, oldRoles); 
						await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), roleId));

						await _context.SaveChangesAsync();

						//user.NormalizedUserName = user.UserName.ToUpper();
						_context.Entry(user).CurrentValues.SetValues(entity);
						await _context.SaveChangesAsync();

						transaction.Commit();
					}
				}
				catch (Exception e)
				{
					transaction.Rollback();
					throw new DbUpdateException("There was an issue deleting item from the Database." + e.Message, e);
				}
			}

			return await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == entity.Id);
		}
	}
}
