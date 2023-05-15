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
        private List<Flight> _flights = new List<Flight>();
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
                flight.Id = _flights.Count;
                _flights.Add(flight);
                
                return flight;
            }
        }

        public void ClearFlights()
        {
            _flights.Clear();
        }

        public bool CheckSameFlights(Flight flight)
        {
            lock (_lock)
            {
                return _context.Flights.Any(f =>
                        flight.ArrivalTime.ToLower().Trim() == f.ArrivalTime.ToLower().Trim() &&
                        flight.Carrier.ToLower().Trim() == f.Carrier.ToLower().Trim() &&
                        flight.DepartureTime.ToLower().Trim() == f.DepartureTime.ToLower().Trim() &&
                        flight.From.AirportCode.ToLower().Trim() == f.From.AirportCode.ToLower().Trim() &&
                        flight.From.City.ToLower().Trim() == f.From.City.ToLower().Trim() &&
                        flight.From.Country.ToLower().Trim() == f.From.Country.ToLower().Trim() &&
                        flight.To.AirportCode.ToLower().Trim() == f.To.AirportCode.ToLower().Trim() &&
                        flight.To.City.ToLower().Trim() == f.To.City.ToLower().Trim() &&
                        flight.To.Country.ToLower().Trim() == f.To.Country.ToLower().Trim());
            }
        }

        public string ToLowercaseTrim(string s)
        {
            if (s != null)
            {
                return s!.ToLower().Trim();
            }

            return s;
        }

        public void DeleteFlight(int id)
        {
            lock (_lock)
            {
                if (id <= _context.Flights.Count() && id >= 0)
                {
                    /*for (int i = 0; i < _context.Flights.Count(); i++)
                    {
                        if (id == _context.Flights[i].Id)
                        {
                            flightIndex = i;
                        }
                    }*/
                    var flightToDelete = _context.Flights.SingleOrDefault(f => f.Id == id);

                    _flights.RemoveAt(flightToDelete.Id);
                }
            }
        }

        public Airport[] SearchAirport(string search)
        {
            var airports = new List<Airport>();
            search = search.ToLower().Trim();
            lock (_lock)
            {
                foreach (var flight in _flights)
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
                }
            }

            return airports.ToArray();
        }

        public PageResult SearchFlight(SearchFlightsRequest search)
        {
            var flights = new List<Flight>();

            lock (_lock)
            {
                foreach (var flight in _flights)
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
