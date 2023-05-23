using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Context;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static readonly object _lock = new object();
        private readonly FlightPlannerContext _context;

        public FlightStorage(FlightPlannerContext context)
        {
            _context = context;
        }

        public Flight GetFlight(int id)
        {
            lock (_lock)
            {
                return _context.Flights.Include(f => f.From)
                    .Include(f => f.To)
                    .SingleOrDefault(f => f.Id == id);
            }
        }

        public Flight AddFlight(Flight flight)
        {
            lock (_lock)
            {
                _context.Flights.Add(flight);
                _context.SaveChanges();
            }

            return flight;
        }

        public void Clear()
        {
            lock (_lock)
            {
                _context.Flights.RemoveRange(_context.Flights);
                _context.Airports.RemoveRange(_context.Airports);
                _context.SaveChanges();
            }
        }
        public bool CheckSameFlights(Flight flight)
        {
            lock (_lock)
            {
                if (flight != null && flight.ArrivalTime != null && flight.To != null && flight.From != null && flight.Carrier != null &&
                    flight.DepartureTime != null && flight.From.AirportCode != null &&
                    flight.From.City != null && flight.From.Country != null && flight.To.AirportCode != null &&
                    flight.To.City != null && flight.To.Country != null)
                {
                    if(IsItDuplicate(flight))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsItDuplicate(Flight flight)
        {
            return _context.Flights.Any(f =>
                f.ArrivalTime == flight.ArrivalTime &&
                f.Carrier == flight.Carrier &&
                f.DepartureTime == flight.DepartureTime &&
                f.From.AirportCode == flight.From.AirportCode &&
                f.From.City == flight.From.City &&
                f.From.Country == flight.From.Country &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.To.City == flight.To.City &&
                f.To.Country == flight.To.Country);
        }

        public void DeleteFlight(int id)
        {
            lock (_lock)
            {
                var flightToDelete = _context.Flights
                    .Include(f => f.To)
                    .Include(f =>f.From)
                    .SingleOrDefault(f => f.Id == id);

                if (flightToDelete != null)
                {
                    _context.Flights.Remove(flightToDelete);
                    _context.Airports.Remove(flightToDelete.From);
                    _context.Airports.Remove(flightToDelete.To);
                    _context.SaveChanges();
                }
            }
        }

        public Airport[] SearchAirport(string search)
        {
            var airports = new List<Airport>();
            search = search.ToLower().Trim();
            lock (_lock)
            {
                airports = _context.Airports
                    .Where(f => f.City.ToLower().Trim().Contains(search) ||
                                f.Country.ToLower().Trim().Contains(search) ||
                                f.AirportCode.ToLower().Trim().Contains(search)).ToList();
            }

            return airports.ToArray();
        }

        public PageResult SearchFlight(SearchFlightsRequest search)
        {
            var flights = new List<Flight>();

            lock (_lock)
            {
                foreach (var flight in _context.Flights.Include(f => f.To).Include(f => f.From))
                {
                    if (search.To == flight.To.AirportCode &&
                        search.From == flight.From.AirportCode &&
                        search.DepartureDate == flight.DepartureTime.Substring(0, 10))
                    {
                        flights.Add(flight);
                    }
                }
            }

            return new PageResult(flights.Count, flights.ToArray());
        }
    }
}
