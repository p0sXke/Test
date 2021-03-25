using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Booking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserProfilesController : Controller
    {
        private readonly IHubContext<SignalrServer> _signalrHub;
        private readonly IUserProfileService _userProfileService;

        public UserProfilesController(IHubContext<SignalrServer> signalrHub, IUserProfileService userProfileService)
        {
            _signalrHub = signalrHub;
            _userProfileService = userProfileService;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _userProfileService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userProfileService.GetAll();
            return Ok(result);
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _userProfileService.Get(id.Value);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleId,FirstName,LastName,Email,UserName,PhoneNumber,Notification")] UserProfileDto userProfile)
        {
            if (ModelState.IsValid)
            {
                long id = await _userProfileService.Create(userProfile);

                if (id == 0)
                    return Conflict();

                await _signalrHub.Clients.All.SendAsync("LoadUserProfiles");
                return RedirectToAction(nameof(Index));
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _userProfileService.Get(id.Value);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleId,FirstName,LastName,Email,UserName,PhoneNumber,Notification")] UserProfileDto userProfile)
        {
            if (id != userProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userProfile = await _userProfileService.Update(userProfile);
                    await _signalrHub.Clients.All.SendAsync("LoadUserProfiles");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _userProfileService.Get(userProfile.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _userProfileService.Get(id.Value);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userProfileService.Delete(id);
            await _signalrHub.Clients.All.SendAsync("LoadUserProfiles");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangePassword()
		{
            return View();
		}

        public IActionResult ReturnToFlights()
        {
            return RedirectToAction("Index", "Flights");
        }        		
	}
}
