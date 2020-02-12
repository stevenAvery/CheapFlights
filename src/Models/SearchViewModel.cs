using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CheapFlights.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Origin is required.")]
        [DisplayName("Origin")]
        public string SelectedOriginId { get; set; }

        [Required(ErrorMessage = "Destination is required.")]
        [DisplayName("Destination")]
        public string SelectedDestinationId { get; set; }

        public IEnumerable<AirportModel> Airports { get; set; }

        public IEnumerable<ItineraryViewModel> Itineraries { get; set; }
    }
}