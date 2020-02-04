using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CheapFlights.Models
{
    public class SearchModel
    {
        [Required(ErrorMessage = "Origin is required.")]
        [DisplayName("Origin")]
        public string SelectedOriginId { get; set; }

        [Required(ErrorMessage = "Destination is required.")]
        [NotEqualTo(nameof(SelectedOriginId), "Destination cannot be the same as Origin.")]
        [DisplayName("Destination")]

        public string SelectedDestinationId { get; set; }

        public IEnumerable<AirportModel> Airports { get; set; }

        public IEnumerable<FlightModel> Flights { get; set; }
    }
}