using System.ComponentModel.DataAnnotations;

namespace CheapFlights.Models {
    public class AirportModel {
        [Key]
        [StringLength(3)]
        public string IataCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }
    }
}