using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

namespace CheapFlights.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Origin is required.")]
        [DisplayName("Origin")]
        public string SelectedOriginId { get; set; }

        [Required(ErrorMessage = "Destination is required.")]
        [NotEqualTo(nameof(SelectedOriginId), ErrorMessage = "Destination cannot be the same as Origin.")]
        [DisplayName("Destination")]
        public string SelectedDestinationId { get; set; }

        public IEnumerable<AirportModel> Airports { get; set; }

        public IEnumerable<ItineraryViewModel> Itineraries { get; set; }
    }
}