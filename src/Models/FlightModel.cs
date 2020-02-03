using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheapFlights.Models {
    public class FlightModel {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public AirportModel Origin { get; set; }

        [Required]
        public AirportModel Destination { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}