using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheapFlights.Models;

namespace CheapFlights.Controllers
{
    [Route("Flights")]
    public class FlightsController : Controller
    {
        [HttpGet, Route("Data")]
        public IActionResult AllFlights()
        {
            var londonAirport = new AirportModel() {
                City = "London",
                IataCode = "YXU"
            };
            var torontoAirport = new AirportModel() {
                City = "Toronto",
                IataCode = "YYZ"
            };
            var newYorkAirport = new AirportModel() {
                City = "New York",
                IataCode = "JFK"
            };

            var data = new List<FlightModel>() {
                new FlightModel() {
                    Origin = londonAirport,
                    Destination = torontoAirport,
                    Cost = 150.00M
                },
                new FlightModel() {
                    Origin = torontoAirport,
                    Destination = newYorkAirport,
                    Cost = 200.00M
                },
                new FlightModel() {
                    Origin = londonAirport,
                    Destination = newYorkAirport,
                    Cost = 400.00M
                }
            };

            return new JsonResult(new { data = data });
        }

        [HttpGet, Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,  Route("Search")]
        public IActionResult Search()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
