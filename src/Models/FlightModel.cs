using System;
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

        [Required]
        public TimeSpan Duration { get; set; }

        public override string ToString() => 
            $"{Origin.ToString()}\n{Destination.ToString()}\n{Cost.ToString()}\n{Duration.ToString()}";
            
    }
}