using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CostumerApiController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            return Ok(FlightStorage.SearchAirport(search));
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest search)
        {
            if (search.To == null || search.From == null || search.DepartureDate == null)
            {
                return BadRequest();
            }

            if (search.From == search.To)
            {
                return BadRequest();
            }

            return Ok(FlightStorage.SearchFlight(search));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightById(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if(flight == null) 
                return NotFound();
            
            return Ok(flight);
        }
    }
}