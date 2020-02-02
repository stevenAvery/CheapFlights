using System;
using System.Collections.Generic;
using System.Linq;
using CheapFlights.Models;

namespace CheapFlights.Repositories {
    public class FlightsRepository {
        public List<AirportModel> GetAllAirports() {
            return new List<AirportModel>() {
                new AirportModel() {
                    IataCode = "YXU",
                    City = "London"
                },
                new AirportModel() {
                    IataCode = "YYZ",
                    City = "Toronto"
                },
                new AirportModel() {
                    IataCode = "JFK",
                    City = "New York"
                }
            };
        }

        public List<FlightModel> GetAllFlights() {
            var allAirports = GetAllAirports();
            var londonAirport  = allAirports.Where(a => a.IataCode == "YXU").First();
            var torontoAirport = allAirports.Where(a => a.IataCode == "YYZ").First();
            var newYorkAirport = allAirports.Where(a => a.IataCode == "JFK").First();

            return new List<FlightModel>() {
                new FlightModel() {
                    Origin = londonAirport,
                    Destination = torontoAirport,
                    Cost = 150.00M
                },
                new FlightModel() {
                    Origin = torontoAirport,
                    Destination = newYorkAirport,
                    Cost = 200.00M
                },
                new FlightModel() {
                    Origin = londonAirport,
                    Destination = newYorkAirport,
                    Cost = 400.00M
                }
            };
        }
    }
}