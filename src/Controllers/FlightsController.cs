using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheapFlights.Models;
using CheapFlights.Repositories;
using CheapFlights.Helpers;
using System.Collections.Generic;

namespace CheapFlights.Controllers {
    [Route("")]
    public class FlightsController : Controller {
        // maintain dollar weight at 1 to keep all weights representitive of dollars
        private const double _DollarWeight = 1.0; 
        // the amount that an average person would be willing to pay to save an hour in the air 
        private const double _HourWeight = 40.0;
        // the amount that an average person would be willing to pay to save a segment on their itinerary
        private const double _SegmentWeight = 200.0;

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
        /// Get all flight data in the application database, with paging, sorting, and searching.
        /// </summary>
        /// <param name="sentParams">Object to specify paging, sorting, and searching of flight data.</param>
        /// <returns>Filtered list of flights in the application database.</returns>
        [HttpGet, Route("Flights/Data")]
        public async Task<IActionResult> FlightsData(DataTablesParamsModel sentParams) {
            var flights = await _flightsRepository.GetAllFlights();

            var filteredData = flights.SearchFilter(sentParams.SearchValue);
            filteredData = filteredData.OrderByColumn(new Dictionary<int, Func<FlightModel, object>>() {
                { 0, flight => flight.Origin.City },
                { 1, flight => flight.Origin.IataCode },
                { 2, flight => flight.Destination.City },
                { 3, flight => flight.Destination.IataCode },
                { 4, flight => flight.Duration.Ticks },
                { 5, flight => flight.Cost },
            }, sentParams.OrderColumn, sentParams.OrderDirection == "asc");

            var paginatedData = filteredData.Paginate(sentParams.Start, sentParams.Length);
            
            return new JsonResult(new DataTablesResultModel<FlightModel>() {
                Draw = sentParams.Draw,
                RecordsTotal = flights.Count(),
                RecordsFiltered = filteredData.Count(),
                Data = paginatedData
            });
        }

        /// <summary>
        /// Action to search for cheapest path from any pair of airports in the application database.
        /// </summary>
        /// <returns>Search view with list of airports in application database.</returns>
        [HttpGet, Route("Search")]
        public async Task<IActionResult> Search() {
            var airports = await _flightsRepository.GetAllAirports();
            return View(new SearchViewModel() {
                Airports = airports
            });
        }

        /// <summary>
        /// Action to search for cheapest path from specific pair of airports in the application database.
        /// </summary>
        /// <param name="search">Contains origin and destination for search.</param>
        /// <returns>Search view with list of airports in application database, and cheapest path from origin to detination.</returns>
        [HttpPost, Route("Search")]
        public async Task<IActionResult> Search(SearchViewModel search) {

            if (search.SelectedOriginId == null || search.SelectedDestinationId == null || 
                search.SelectedOriginId == search.SelectedDestinationId)
                return PartialView("_Itineraries", new List<ItineraryViewModel>());

            // get all paths from origin airport to destination airport
            var paths = (await _flightsRepository.GetAllFlights())
                .ToAdjacencyList(flight => flight.Origin.IataCode, flight => flight.Destination.IataCode)
                .AllPaths(search.SelectedOriginId, search.SelectedDestinationId);
            
            // find noteworthy paths
            var bestDeal = paths.Min(flights => flights.Sum(flight => flight.Cost));
            var shortestDuration = paths.Min(flights => flights.Sum(flight => flight.Duration.Ticks));

            // generate flight itineraries from paths
            var itineraries = paths
                .Select(flights => {
                    var totalCost = flights.Sum(flight => flight.Cost);
                    var totalDurationTicks = flights.Sum(flight => flight.Duration.Ticks);
                    return new ItineraryViewModel() {
                        TotalCost = totalCost,
                        TotalDuration = new TimeSpan(totalDurationTicks),
                        Flights = flights,
                        IsSuggested = false,
                        IsBestDeal = totalCost == bestDeal,
                        IsShortestDuration = totalDurationTicks == shortestDuration,
                        IsDirect = flights.Count() == 1
                    };
                })
                .OrderBy(itinerary => 
                    _DollarWeight * (double)itinerary.TotalCost +
                    _HourWeight * (double)itinerary.TotalDuration.TotalHours +
                    _SegmentWeight * (double)itinerary.Flights.Count())
                .ToList();
            itineraries.First().IsSuggested = true; // suggest the itinerary that is ranked the best

            return PartialView("_Itineraries", itineraries);
        }

        /// <summary>
        /// Shows appropriate error page for a given status code.
        /// Called by error status code re-execute.
        /// </summary>
        /// <param name="statusCode">Error status code that occured.</param>
        /// <returns>error page for the given status code.</returns>
        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode) {
            return View(new ErrorViewModel { 
                StatusCode = statusCode
            });
        }
    }
}
