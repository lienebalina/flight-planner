using FlightPlanner.Context;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CostumerApiController : ControllerBase
    {
        private readonly FlightPlannerContext _context;
        private FlightStorage _storage;
        public CostumerApiController(FlightPlannerContext context)
        {
            _context = context;
            _storage = new FlightStorage(context);
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            return Ok(_storage.SearchAirport(search));
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

            return Ok(_storage.SearchFlight(search));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightById(int id)
        {
            var flight = _storage.GetFlight(id);
            if (flight == null) 
                return NotFound();
            
            return Ok(flight);
        }
    }
}