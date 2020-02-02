using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheapFlights.Models;
using CheapFlights.Repositories;
using CheapFlights.Helpers;

namespace CheapFlights.Controllers {
    [Route("")]
    public class FlightsController : Controller {
        [HttpGet, Route("Data/Flights")]
        public IActionResult AllFlights() {
            return new JsonResult(new { 
                data = new FlightsRepository().GetAllFlights() // TODO dependency injection
            });
        }

        [HttpGet, Route("Index"), Route("")]
        public IActionResult Index() {
            return View();
        }

        [HttpGet, Route("Search")]
        public IActionResult Search() {
            return View(new SearchModel() {
                Airports = new FlightsRepository().GetAllAirports() // TODO dependency injection
            });
        }

        [HttpPost, Route("Search")]
        public IActionResult Search(SearchModel searchModel) {
            if (!ModelState.IsValid)
                return Search();

            var adjList = new FlightsRepository().GetAllFlights().ToAdjacencyList(); // TODO dependency injection
            var result = adjList.ShortestPath(searchModel.SelectedOriginId, searchModel.SelectedDestinationId);

            return new JsonResult(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
