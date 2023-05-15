using System;
using System.Linq;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Checker;
using FlightPlanner.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private readonly FlightPlannerContext _context;
        private FlightStorage _storage;
        public AdminApiController(FlightPlannerContext context)
        {
            _context = context;
            _storage = new FlightStorage(context);
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
            if (flight != null)
            {
                if (_storage.CheckSameFlights(flight))
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

            _context.Flights.Add(flight);
            _context.SaveChanges();

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
