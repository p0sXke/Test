using Booking.Business.Models.Dto;
using Booking.Domain.Model;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly UserManager<UserProfile> _userManager;
		private readonly SignInManager<UserProfile> _signInManager;

		public HomeController(
			UserManager<UserProfile> userManager,
			SignInManager<UserProfile> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		private Task<UserProfile> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		//[Authorize(Roles = "Admin")]
		public IActionResult MainPage()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("Name, Password")] LoginDto login)
		{
			var user = await _userManager.FindByNameAsync(login.Name);
			if (user == null)
				user = _userManager.Users.FirstOrDefault(x => x.Email == login.Name);

			if(user != null)
			{
				if (user.PasswordHash == null)
					await _userManager.AddPasswordAsync(user, login.Password);

				if(_userManager.Users.Count() == 1)
				await _userManager.AddToRoleAsync(user, "Admin");

				var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);

				if (signInResult.Succeeded)
				{
					return RedirectToAction("MainPage");
				}
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index");
		}

		public IActionResult Flights()
		{
			return RedirectToAction("Index", "Flights");
		}
		
		public IActionResult Booking()
		{
			return RedirectToAction("Index", "BookedFlights");
		}
		
		public IActionResult Search()
		{
			return RedirectToAction("Index", "Search");
		}
		
		public IActionResult Users()
		{
			return RedirectToAction("Index", "UserProfiles");
		}

		public IActionResult ChangePassword()
		{
			return View();
		}

		// POST: UserProfiles/ChangePassword/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword([Bind("OldPassword")] string oldPassword, [Bind("NewPassword")] string newPassword)
		{
			var user = await GetCurrentUserAsync();


			if (user != null)
			{
				if (ModelState.IsValid)
				{
					try
					{
						await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

						return RedirectToAction("Logout");
					}
					catch (DbUpdateConcurrencyException)
					{
						return NotFound();
					}
				}
			}
			return RedirectToAction("Index");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
