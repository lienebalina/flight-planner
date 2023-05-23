using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Search;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SearchFlightsRequest = FlightPlanner.Core.Models.SearchFlightsRequest;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CostumerApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ISearch _search;
        private readonly IMapper _mapper;
        public CostumerApiController(IFlightService flightService,
            ISearch search,
            IMapper mapper)
        {
            _flightService = flightService;
            _search = search;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var searchResults = _search.SearchAirports(search);

            var mappedResults = new List<AddAirportRequest>();

            foreach (var result in searchResults)
            {
                var mappedResult = _mapper.Map<AddAirportRequest>(result);
                mappedResults.Add(mappedResult);
            }

            return Ok(mappedResults.ToArray());
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

            return Ok(_search.SearchFlight(search));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlightById(int id)
        {
            var flight = _flightService.GetFullFlight(id);
            if (flight == null) 
                return NotFound();
            
            return Ok(_mapper.Map<AddFlightRequest>(flight));
        }
    }
}