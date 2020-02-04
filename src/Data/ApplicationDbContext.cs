using CheapFlights.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapFlights.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<AirportModel> Airports { get; set; }
        public DbSet<FlightModel> Flights { get; set; }
    }
}