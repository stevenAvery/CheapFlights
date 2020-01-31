namespace CheapFlights.Models
{
    public class FlightModel
    {
        public AirportModel Origin { get; set; }
        public AirportModel Destination { get; set; }
        public decimal Cost { get; set; }
    }
}