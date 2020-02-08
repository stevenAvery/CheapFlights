using System;
using System.Collections.Generic;

namespace CheapFlights.Models
{
    public class ItineraryModel {
        public TimeSpan TotalDuration { get; set; }
        public Decimal TotalCost { get; set; }
        public IEnumerable<FlightModel> Flights { get; set; }

        public bool IsSuggested { get; set; } = false;
        public bool IsBestDeal { get; set; } = false;
        public bool IsDirect { get; set; } = false;
        public bool IsShortestDuration { get; set; } = false;
    }
}