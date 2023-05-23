using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Search;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services.Search
{
    public class Search : ISearch
    {
        private readonly IFlightPlannerContext _context;

        public Search(IFlightPlannerContext context)
        {
            _context = context;
        }
        public Airport[] SearchAirports(string search)
        {
            var airports = new List<Airport>();
            search = search.ToLower().Trim();
            
            airports = _context.Airports
                .Where(f => f.City.ToLower().Trim().Contains(search) ||
                            f.Country.ToLower().Trim().Contains(search) ||
                            f.AirportCode.ToLower().Trim().Contains(search)).ToList();
            

            return airports.ToArray();
        }

        public PageResult SearchFlight(SearchFlightsRequest search)
        {
            var flights = new List<Flight>();
            
            foreach (var flight in _context.Flights.Include(f => f.To).Include(f => f.From))
            {
                if (search.To == flight.To.AirportCode &&
                    search.From == flight.From.AirportCode &&
                    search.DepartureDate == flight.DepartureTime.Substring(0, 10))
                {
                    flights.Add(flight);
                }
            }
            

            return new PageResult(flights.Count, flights.ToArray());
        }
    }
}
