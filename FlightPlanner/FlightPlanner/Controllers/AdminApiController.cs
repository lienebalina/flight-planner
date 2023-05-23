using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Checker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private FlightStorage _storage;
        public AdminApiController(FlightStorage storage)
        {
            _storage = storage;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = _storage.GetFlight(id);
            if(flight == null) 
                return NotFound();

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            if (flight != null )
            {
                if (FlightChecker.IsFlightValueNullOrEmpty(flight))
                {
                    return BadRequest();
                }

                if (FlightChecker.IsItTheSameAirport(flight))
                {
                    return BadRequest();
                }

                if (FlightChecker.AreTheDepartureAndArrivalDateTimesValid(flight))
                { 
                    return BadRequest();
                }

                if (_storage.CheckSameFlights(flight))
                {
                    return Conflict();
                }

                _storage.AddFlight(flight);
            }
            
            return Created("", flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            _storage.DeleteFlight(id);
            return Ok();
        }
    }
}
