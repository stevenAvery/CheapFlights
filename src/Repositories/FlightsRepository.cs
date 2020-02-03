using System;
using System.Collections.Generic;
using System.Linq;
using CheapFlights.Data;
using CheapFlights.Models;

namespace CheapFlights.Repositories {
    public class FlightsRepository : IFlightsRepository {
        private readonly ApplicationDbContext _context;

        public FlightsRepository(ApplicationDbContext context) {
            _context = context;
        }
        
        public IEnumerable<AirportModel> GetAllAirports() {
            return _context.Airports.ToList();
        }

        public IEnumerable<FlightModel> GetAllFlights() {
            var airports = _context.Airports.ToList();
            var flights = _context.Flights.ToList();
            return flights;
        }
    }
}