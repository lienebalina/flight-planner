using System;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Checker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if(flight == null) 
                return NotFound();

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            if (flight != null)
            {
                if (FlightStorage.CheckSameFlights(flight))
                {
                    return Conflict();
                }

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
            }

            flight = FlightStorage.AddFlight(flight);

            return Created("", flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            FlightStorage.DeleteFlight(id);
            return Ok();
        }
    }
}
