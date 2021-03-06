using System;
using System.Linq;
using CheapFlights.Models;

namespace CheapFlights.Data {
    public static class DataInitializer {
        /// <summary>
        /// Create and seed database, if it hasn't been initialized before.
        /// </summary>
        /// <param name="context">Application database context to initialize.</param>
        public static void Initialize(ApplicationDbContext context) {
            context.Database.EnsureCreated();

            if (!context.Airports.Any() && !context.Flights.Any()) {
                // add airports to database if none exist
                context.Airports.Add(new AirportModel() {
                    IataCode = "YYZ",
                    City = "Toronto"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "ATL",
                    City = "Atlanta"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "AMS",
                    City = "Amsterdam"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "DXB",
                    City = "Dubai"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "HND",
                    City = "Tokyo"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "CPT",
                    City = "Cape Town"
                });
                context.Airports.Add(new AirportModel() {
                    IataCode = "DEL",
                    City = "Delhi"
                });

                context.SaveChanges();

                // add flights to database if none exis
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Destination = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Cost = 350.00M,
                    Duration = new TimeSpan(1, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Cost = 1900.00M,
                    Duration = new TimeSpan(9, 18, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Cost = 2200.00M,
                    Duration = new TimeSpan(15, 41, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Cost = 2400.00M,
                    Duration = new TimeSpan(16, 27, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "HND"),
                    Cost = 1800.00M,
                    Duration = new TimeSpan(14, 15, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Destination = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Cost = 3000.00M,
                    Duration = new TimeSpan(16, 44, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Cost = 2300.00M,
                    Duration = new TimeSpan(14, 59, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Destination = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Cost = 1700.00M,
                    Duration = new TimeSpan(7, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "HND"),
                    Destination = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Cost = 1500.00M,
                    Duration = new TimeSpan(13, 23, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Cost = 800.00M,
                    Duration = new TimeSpan(6, 55, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Cost = 2000.00M,
                    Duration = new TimeSpan(8, 25, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Destination = context.Airports.First(airport => airport.IataCode == "HND"),
                    Cost = 1800.00M,
                    Duration = new TimeSpan(12, 5, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Destination = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Cost = 1400.00M,
                    Duration = new TimeSpan(9, 55, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Cost = 1900.00M,
                    Duration = new TimeSpan(3, 15, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "HND"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Cost = 1800.00M,
                    Duration = new TimeSpan(7, 46, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Destination = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Cost = 2000.00M,
                    Duration = new TimeSpan(9, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Destination = context.Airports.First(airport => airport.IataCode == "HND"),
                    Cost = 2000.00M,
                    Duration = new TimeSpan(10, 23, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "HND"),
                    Destination = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Cost = 1700.00M,
                    Duration = new TimeSpan(12, 5, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DXB"),
                    Destination = context.Airports.First(airport => airport.IataCode == "AMS"),
                    Cost = 900.00M,
                    Duration = new TimeSpan(6, 55, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Cost = 400.00M,
                    Duration = new TimeSpan(1, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "DEL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Cost = 2200.00M,
                    Duration = new TimeSpan(16, 27, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Cost = 4000.00M,
                    Duration = new TimeSpan(15, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "ATL"),
                    Destination = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Cost = 4600.00M,
                    Duration = new TimeSpan(15, 58, 0)
                });
                context.Flights.Add(new FlightModel() {
                    Origin      = context.Airports.First(airport => airport.IataCode == "YYZ"),
                    Destination = context.Airports.First(airport => airport.IataCode == "CPT"),
                    Cost = 4400.00M,
                    Duration = new TimeSpan(16, 1, 0)
                });
                context.SaveChanges();
            }
        }
    }
}