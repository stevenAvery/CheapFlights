using CheapFlights.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapFlights.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder options) => 
        //     options.UseSqlite("Data Source=CheapFlights.db");

        public DbSet<AirportModel> Airports { get; set; }
        public DbSet<FlightModel> Flights { get; set; }
    }
}