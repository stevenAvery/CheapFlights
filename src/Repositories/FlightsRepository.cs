using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheapFlights.Data;
using CheapFlights.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapFlights.Repositories {
    public class FlightsRepository : IFlightsRepository {
        private readonly ApplicationDbContext _context;

        public FlightsRepository(ApplicationDbContext context) {
            _context = context;
        }
        
        /// <summary>
        /// List all airports in the application database.
        /// </summary>
        /// <returns>List of all airports in the application database.</returns>
        public async Task<IEnumerable<AirportModel>> GetAllAirports() {
            return await _context.Airports.ToListAsync();
        }

        /// <summary>
        /// List all flights in the application database.
        /// </summary>
        /// <returns>List of all flights, with related airport data in the application database.</returns>
        public async Task<IEnumerable<FlightModel>> GetAllFlights() {
            return await _context.Flights
                .Include(c => c.Origin) // load related data
                .Include(c => c.Destination)
                .ToListAsync();
        }
    }
}