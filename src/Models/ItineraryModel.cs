using System;
using System.Collections.Generic;

namespace CheapFlights.Models
{
    public class ItineraryModel {
        public TimeSpan TotalDuration { get; set; }
        public Decimal TotalCost { get; set; }
        public IEnumerable<FlightModel> Flights { get; set; }
        public Boolean IsBestDeal { get; set; } = false;
        public Boolean IsDirect { get; set; } = false;
        public Boolean IsShortestDuration { get; set; } = false;
    }
}