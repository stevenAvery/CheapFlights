using System.Collections.Generic;
using System.Threading.Tasks;
using CheapFlights.Models;

namespace CheapFlights.Repositories {
    public interface IFlightsRepository {
        Task<IEnumerable<AirportModel>> GetAllAirports();
        Task<IEnumerable<FlightModel>> GetAllFlights();
    }
}