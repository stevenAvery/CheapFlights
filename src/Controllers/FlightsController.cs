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

        /// <summary>
        /// Action to get all flight data in the application database.
        /// </summary>
        /// <returns>Flight view with list of flights in application database.</returns>
        [HttpGet, Route("Flights"), Route("")]
        public async Task<IActionResult> Flights() {
            var flights = await _flightsRepository.GetAllFlights();
            return View(flights);
        }

        /// <summary>
        /// Action to search for cheapest path from any pair of airports in the application database.
        /// </summary>
        /// <returns>Search view with list of airports in application database.</returns>
        [HttpGet, Route("Search")]
        public async Task<IActionResult> Search() {
            var airports = await _flightsRepository.GetAllAirports();
            return View(new SearchModel() {
                Airports = airports
            });
        }

        /// <summary>
        /// Action to search for cheapest path from specific pair of airports in the application database.
        /// </summary>
        /// <param name="searchModel">Contains origin and destination for search.</param>
        /// <returns>Search view with list of airports in application database, and cheapest path from origin to detination.</returns>
        [HttpPost, Route("Search")]
        public async Task<IActionResult> Search(SearchModel searchModel) {
            if (!ModelState.IsValid)
                return await Search();

            var airports = await _flightsRepository.GetAllAirports();
            var flights = await _flightsRepository.GetAllFlights();
            var cheapestPath = flights
                .ToAdjacencyList(flight => flight.Cost)
                .ShortestPath(searchModel.SelectedOriginId, searchModel.SelectedDestinationId,
                    0, decimal.MaxValue, (a, b) => a + b)
                .Select(edge => new FlightModel() {
                    Origin      = airports.First(airport => airport.IataCode == edge.Origin),
                    Destination = airports.First(airport => airport.IataCode == edge.Destination),
                    Cost = edge.Distance
                })
                .ToList();
            
            return View(new SearchModel() {
                Airports = airports,
                Flights = cheapestPath
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
