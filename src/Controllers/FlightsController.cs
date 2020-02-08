﻿using System;
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
            var paths = (await _flightsRepository.GetAllFlights())
                .ToAdjacencyList(flight => flight.Origin.IataCode, flight => flight.Destination.IataCode)
                .AllPaths(searchModel.SelectedOriginId, searchModel.SelectedDestinationId);
            
            var bestDeal = paths.Min(flights => flights.Sum(flight => flight.Cost));
            var shortestDuration = paths.Min(flights => flights.Sum(flight => flight.Duration.Ticks));

            var itineraries = paths
                .Select((flights, index) => {
                    var totalCost = flights.Sum(flight => flight.Cost);
                    var totalDurationTicks = flights.Sum(flight => flight.Duration.Ticks);
                    return new ItineraryModel() {
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

            return View(new SearchModel() {
                Airports = airports,
                Itineraries = itineraries
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
