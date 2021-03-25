using Booking.Business.Models.Request;
using Booking.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Booking.Controllers
{
	public class SearchController : Controller
	{
		private readonly ISearchService _searchService;

		public SearchController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		public IActionResult Index()
		{
			return View();
		}

		// GET: Search/Search
		[HttpGet]
		public IActionResult Search()
		{
			return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Search([Bind("DepartureId,DestinationId,NoStops")] FlightSearchRequest searchRequest)
		{
			if (ModelState.IsValid)
			{
				var result = await _searchService.GetFlightsSearchResult(searchRequest);
				return View(result);

			}
			return View("Index");
		}

		public ActionResult ReturnToMain()
		{
			return RedirectToAction("MainPage", "Home");
		}
	}
}
