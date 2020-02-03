using System.Collections.Generic;
using CheapFlights.Models;

namespace CheapFlights.Repositories {
    public interface IFlightsRepository {
        IEnumerable<AirportModel> GetAllAirports();
        IEnumerable<FlightModel> GetAllFlights();
    }
}