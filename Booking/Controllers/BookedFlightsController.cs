using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Booking.Domain.Model;
using Booking.Domain.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Booking.Controllers
{
    [Authorize]
	public class BookedFlightsController : Controller
    {
        private readonly IHubContext<SignalrServer> _signalrHub;
        private readonly IBookedFlightService _service;
        private readonly IFlightService _flightService;
		private readonly UserManager<UserProfile> _userManager;

		public BookedFlightsController(IHubContext<SignalrServer> signalrHub, IBookedFlightService service, IFlightService flightService, UserManager<UserProfile> userManager)
        {
            _signalrHub = signalrHub;
            _service = service;
            _flightService = flightService;
            _userManager = userManager;
        }

        private Task<UserProfile> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: BookedFlights
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Index()
        {
			return View(await _service.GetAll());
		}

        public IActionResult ReturnToFlights()
		{
            return RedirectToAction("Index", "Flights");
        }

        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> ApproveReject(int? id)
		{
            if (id == null)
            {
                return NotFound();
            }

            var bookedFlight = await _service.Get(id.Value);

            if (bookedFlight == null)
            {
                return NotFound();
            }

            return View(bookedFlight);
        }

        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> ApproveBooking(int id)
		{
            await _service.ChangeBookingStatus(id, BookingStatusEnum.Approved);
            return RedirectToAction("Index");
		}

        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> RejectBooking(int id)
        {
            await _service.ChangeBookingStatus(id, BookingStatusEnum.Rejected);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersBookedFlights()
		{
            var user = await GetCurrentUserAsync();

            var result = await _service.GetUsersBookedFlights(user.Id);
            return View(result);
		}

        [Authorize(Roles = "Admin, Agent")]
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        // GET: BookedFlights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookedFlight = await _service.Get(id.Value);

            if (bookedFlight == null)
            {
                return NotFound();
            }

            return View(bookedFlight);
        }

        // GET: BookedFlights/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightService.Get(id.Value);

            if (flight == null || flight.Canceled)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: BookedFlights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id")] int flightId, [Bind("BookSeats")] int bookSeats)
        {
            BookedFlightDto bookedFlight = new BookedFlightDto();

            var user = await GetCurrentUserAsync();
            if (user != null)
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {

                            bookedFlight = await _service.Create(id, bookSeats, user.Id);
                            await _signalrHub.Clients.All.SendAsync("LoadBookings");
                        }
                        catch (Exception e)
                        {
                            TempData["Message"] = e.Message;
                            return View("Details");
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.Get(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View("Details", bookedFlight);
        }

        // GET: BookedFlights/Edit/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var bookedFlight = await _service.Get(id.Value);
                if (bookedFlight == null)
                {
                    return NotFound();
                }
                return View(bookedFlight);
            }
        }

        // POST: BookedFlights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(int id, [Bind("BookSeats")] int bookSeats)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bookedFlight = await _service.Update(id, bookSeats);
                    await _signalrHub.Clients.All.SendAsync("LoadBookings");
                    return View("Details", bookedFlight);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.Get(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
                       
            return View();
        }

        // GET: BookedFlights/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            else
            {
                var bookedFlight = await _service.Get(id.Value);
                if (bookedFlight == null)
                {
                    return NotFound();
                }

                return View(bookedFlight);

            }
        }

        // POST: BookedFlights/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.Delete(id);
            await _signalrHub.Clients.All.SendAsync("LoadBookings");
            return RedirectToAction(nameof(Index));
        }
    }
}
