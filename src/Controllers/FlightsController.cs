using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheapFlights.Models;
using CheapFlights.Repositories;
using CheapFlights.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace CheapFlights.Controllers {
    [Route("")]
    public class FlightsController : Controller {
        // maintain dollar weight at 1 to keep all weights representitive of dollars
        private const double DollarWeight = 1.0; 
        // the amount that an average person would be willing to pay to save an hour in the air 
        private const double HourWeight = 25.0;
        // the amount that an average person would be willing to pay to save a segment on their itinerary
        private const double SegmentWeight = 150.0;

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
            var data = await _flightsRepository.GetAllFlights();

            // TODO make this work on generic objects to reduce coupling / hardcoding
            CultureInfo enCulture = new CultureInfo("en-CA", false);
            var filteredData = String.IsNullOrWhiteSpace(sentParams.SearchValue)
                ? data
                : data.Where(flight => enCulture.CompareInfo.IndexOf(
                    flight.ToString(), sentParams.SearchValue, CompareOptions.IgnoreCase) >= 0);

            // TODO better way to map int id to object property to reduce coupling / hardcoding
            switch (sentParams.OrderColumn) {
                case 0: filteredData = filteredData.OrderBy(flight => flight.Origin.City); break;
                case 1: filteredData = filteredData.OrderBy(flight => flight.Origin.IataCode); break;
                case 2: filteredData = filteredData.OrderBy(flight => flight.Destination.City); break;
                case 3: filteredData = filteredData.OrderBy(flight => flight.Destination.IataCode); break;
                case 4: filteredData = filteredData.OrderBy(flight => flight.Duration.Ticks); break;
                case 5: filteredData = filteredData.OrderBy(flight => flight.Cost); break;
            }
            if (sentParams.OrderDirection == "desc")
                filteredData = filteredData.Reverse();

            var paginatedData = filteredData.Skip(sentParams.Start).Take(sentParams.Length);
            
            return new JsonResult(new DataTablesResultModel<FlightModel>() {
                Draw = sentParams.Draw,
                RecordsTotal = data.Count(),
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
            if (!ModelState.IsValid)
                return await Search();

            var airports = await _flightsRepository.GetAllAirports();
            var paths = (await _flightsRepository.GetAllFlights())
                .ToAdjacencyList(flight => flight.Origin.IataCode, flight => flight.Destination.IataCode)
                .AllPaths(search.SelectedOriginId, search.SelectedDestinationId);
            
            var bestDeal = paths.Min(flights => flights.Sum(flight => flight.Cost));
            var shortestDuration = paths.Min(flights => flights.Sum(flight => flight.Duration.Ticks));

            var itineraries = paths
                .Select((flights, index) => {
                    var totalCost = flights.Sum(flight => flight.Cost);
                    var totalDurationTicks = flights.Sum(flight => flight.Duration.Ticks);
                    return new ItineraryViewModel() {
                        TotalCost = totalCost,
                        TotalDuration = new TimeSpan(totalDurationTicks),
                        Flights = flights,
                        IsSuggested = index == 0,
                        IsBestDeal = totalCost == bestDeal,
                        IsShortestDuration = totalDurationTicks == shortestDuration,
                        IsDirect = flights.Count() == 1
                    };
                })
                .OrderBy(itinerary => 
                    DollarWeight * (double)itinerary.TotalCost +
                    HourWeight * (double)itinerary.TotalDuration.TotalHours +
                    SegmentWeight * (double)itinerary.Flights.Count())
                .ToList();

            return View(new SearchViewModel() {
                Airports = airports,
                Itineraries = itineraries
            });
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
                StatusCode = statusCode,
                ReasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode)
            });
        }
    }
}
