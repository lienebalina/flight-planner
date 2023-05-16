using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Context;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private readonly object _lock = new object();
        private FlightPlannerContext _context { get; set; }

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

                return flight;
            }
        }

        public void Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
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
                    /*var existing = _context.Flights
                        .Include(f => f.From)
                        .Include(f => f.To)
                        .FirstOrDefault(f => f.From.AirportCode == flight.From.AirportCode && 
                                             f.To.AirportCode == flight.To.AirportCode && 
                                             f.Carrier == flight.Carrier &&
                                             f.DepartureTime == flight.DepartureTime &&
                                             f.ArrivalTime == flight.ArrivalTime);

                    if (existing != null) return true;*/
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
                if (id <= _context.Flights.Count() && id >= 0)
                {
                    var flightToDelete = _context.Flights.Include(f => f.To)
                        .Include(f =>f.From).SingleOrDefault(f => f.Id == id);

                    _context.Flights.Remove(flightToDelete);
                }
            }
        }

        public Airport[] SearchAirport(string search)
        {
            var airports = new List<Airport>();
            search = search.ToLower().Trim();
            lock (_lock)
            {
                Airport airportTo = (Airport)_context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .Where(f => f.To.City.ToLower().Trim().Contains(search) ||
                                f.To.Country.ToLower().Trim().Contains(search) ||
                                f.To.AirportCode.ToLower().Trim().Contains(search));
                airports.Add(airportTo);

                Airport airportFrom = (Airport)_context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .Where(f => f.From.City.ToLower().Trim().Contains(search) ||
                                f.From.Country.ToLower().Trim().Contains(search) ||
                                f.From.AirportCode.ToLower().Trim().Contains(search));
                airports.Add(airportFrom);
                /*
                foreach (var flight in _context.Flights.Include(f => f.To).Include(f => f.From))
                {
                    if (flight.To.Country.ToLower().Trim().Contains(search) ||
                        flight.To.AirportCode.ToLower().Trim().Contains(search) ||
                        flight.To.City.ToLower().Trim().Contains(search))
                    {
                        airports.Add(flight.To);
                    }

                    if (flight.From.Country.ToLower().Trim().Contains(search) ||
                        flight.From.AirportCode.ToLower().Trim().Contains(search) ||
                        flight.From.City.ToLower().Trim().Contains(search))
                    {
                        airports.Add(flight.From);
                    }
                }*/
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
