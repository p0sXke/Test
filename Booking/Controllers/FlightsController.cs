using Booking.Business.Models.Dto;
using Booking.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Booking.Controllers
{
	public class FlightsController : Controller
	{
		private readonly IHubContext<SignalrServer> _signalrHub;
		private readonly IFlightService _service;

		public FlightsController(IHubContext<SignalrServer> signalrHub, IFlightService service)
		{
			_signalrHub = signalrHub;
			_service = service;
		}

		// GET: Flights
		public async Task<IActionResult> Index()
		{
			return View(await _service.GetAll());
		}

		[HttpGet]
		public async Task<IActionResult> GetFlights()
		{
			var result = await _service.GetAll();
			return Ok(result);
		}
		
		[HttpGet]
		public IActionResult GetUsersBookedFlights()
		{
			return RedirectToAction("GetUsersBookedFlights", "BookedFlights");
		}

		// GET: Flights/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var flight = await _service.Get(id.Value);

			if (flight == null)
			{
				return NotFound();
			}

			return View(flight);
		}

		// GET: Flights/Create
		[Authorize(Roles = "Admin, Agent")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Flights/Create
		[Authorize(Roles = "Admin, Agent")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,DepartureId,DestinationId,DateTime,StopNumber,SeatNumber")] FlightDto flight)
		{
			if (ModelState.IsValid)
			{
				long id = await _service.Create(flight);

				if (id == 0)
					return Conflict();

				await _signalrHub.Clients.All.SendAsync("LoadFlights");
				return RedirectToAction(nameof(Index));
			}
			return View(flight);
		}

		// GET: Flights/Edit/5
		[Authorize(Roles = "Admin, Agent")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			else
			{
				var flight = await _service.Get(id.Value);
				if (flight == null)
				{
					return NotFound();
				}
				return View(flight);
			}
		}

		// POST: Flights/Edit/5
		[Authorize(Roles = "Admin, Agent")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,DepartureId,DestinationId,DateTime,StopNumber,SeatNumber")] FlightDto flight)
		{
			if (id != flight.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					flight = await _service.Update(flight);
					await _signalrHub.Clients.All.SendAsync("LoadFlights");
				}
				catch (DbUpdateConcurrencyException)
				{
					if (await _service.Get(flight.Id) == null)
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
			return View(flight);
		}

		// GET: Flights/Delete/5
		[Authorize(Roles = "Admin, Agent")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			else
			{
				var flight = await _service.Get(id.Value);
				if (flight == null)
				{
					return NotFound();
				}

				return View(flight);
			}
		}

		// POST: Flights/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin, Agent")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _service.Delete(id);
			await _signalrHub.Clients.All.SendAsync("LoadFlights");
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> BookFlight(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var flight = await _service.Get(id.Value);

			if (flight == null)
			{
				return NotFound();
			}

			return RedirectToAction("Create", "BookedFlights", flight);
		}

		public ActionResult ReturnToMain()
		{
			return RedirectToAction("MainPage", "Home");
		}

		// POST: Flights/Delete/5
		[HttpPost, ActionName("CancelFlight")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CancelFlight(int id)
		{
			await _service.CancelFlight(id);
			await _signalrHub.Clients.All.SendAsync("LoadFlights");
			return RedirectToAction(nameof(Index));
		}
	}
}
