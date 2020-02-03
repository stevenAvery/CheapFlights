using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CheapFlights.Models
{
    public class SearchModel
    {
        [Required]
        [DisplayName("Origin")]
        public string SelectedOriginId { get; set; }

        [Required]
        [DisplayName("Destination")]
        // TODO opposite of "Compare"
        // [Compare(nameof(SelectedOriginId), ErrorMessage = "Origin must not be the same as the destination.")]
        public string SelectedDestinationId { get; set; }

        public IEnumerable<AirportModel> Airports { get; set; }

        public IEnumerable<FlightModel> Flights { get; set; }
    }
}