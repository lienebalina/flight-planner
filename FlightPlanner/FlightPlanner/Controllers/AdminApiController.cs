using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.Deletion;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Validations;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly IMapper _mapper;
        private readonly IEnumerable<IValidate> _validators;
        private readonly IFlightService _flightService;
        private readonly IFlightDelete _flightDelete;
        public AdminApiController(IFlightService flightService,
            IMapper mapper,
            IEnumerable<IValidate> validators,
            IFlightDelete flightDelete)
        {
            _mapper = mapper;
            _validators = validators;
            _flightService = flightService;
            _flightDelete = flightDelete;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = _flightService.GetFullFlight(id);
            if(flight == null) 
                return NotFound();

            return Ok(_mapper.Map<AddFlightRequest>(flight));
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest request)
        {
            var flight = _mapper.Map<Flight>(request);

            lock (_lock)
            {
                if (!_validators.All(v => v.IsValid(flight)))
                {
                    return BadRequest();
                }

                if (_flightService.FlightExists(flight))
                {
                    return Conflict();
                }

                _flightService.Create(flight);
            }

            return Created("", _mapper.Map<AddFlightRequest>(flight));
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_lock)
            {
                _flightDelete.Delete(id);
            }

            return Ok();
        }
    }
}
