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
        private readonly IFlightsRepository _flightsRepository;

        public FlightsController(IFlightsRepository flightsRepository) {
            _flightsRepository = flightsRepository;
        }

        [HttpGet, Route("Data/Flights")]
        public IActionResult AllFlights() {
            return new JsonResult(new { 
                data = _flightsRepository.GetAllFlights()
            });
        }

        [HttpGet, Route("Index"), Route("")]
        public IActionResult Index() {
            return View();
        }

        [HttpGet, Route("Search")]
        public IActionResult Search() {
            return View(new SearchModel() {
                Airports = _flightsRepository.GetAllAirports()
            });
        }

        [HttpPost, Route("Search")]
        public IActionResult Search(SearchModel searchModel) {
            if (!ModelState.IsValid)
                return Search();

            var cheapestPath = _flightsRepository
                .GetAllFlights()
                .ToAdjacencyList()
                .ShortestPath(searchModel.SelectedOriginId, searchModel.SelectedDestinationId);
            
            return new JsonResult(cheapestPath);
            // return View(new SearchModel() {
            //     Airports = _flightsRepository.GetAllAirports(),
            //     Flights = cheapestPath
            // });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
