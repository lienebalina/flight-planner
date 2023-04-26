using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static readonly object _lock = new object();
        
        public static Flight GetFlight(int id)
        {
            lock (_lock)
            {
                return _flights.SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static Flight AddFlight(Flight flight)
        {
            lock (_lock)
            {
                flight.Id = _flights.Count;
                _flights.Add(flight);
                
                return flight;
            }
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static bool CheckSameFlights(Flight flight)
        {
            lock (_lock)
            {
                foreach (var f in _flights)
                {
                    if (ToLowercaseTrim(flight.ArrivalTime) == ToLowercaseTrim(f.ArrivalTime) &&
                        ToLowercaseTrim(flight.Carrier) == ToLowercaseTrim(f.Carrier) &&
                        ToLowercaseTrim(flight.DepartureTime) == ToLowercaseTrim(f.DepartureTime) &&
                        ToLowercaseTrim(flight.From.AirportCode) == ToLowercaseTrim(f.From.AirportCode) &&
                        ToLowercaseTrim(flight.From.City) == ToLowercaseTrim(f.From.City) &&
                        ToLowercaseTrim(flight.From.Country) == ToLowercaseTrim(f.From.Country) &&
                        ToLowercaseTrim(flight.To.AirportCode) == ToLowercaseTrim(f.To.AirportCode) &&
                        ToLowercaseTrim(flight.To.City) == ToLowercaseTrim(f.To.City) &&
                        ToLowercaseTrim(flight.To.Country) == ToLowercaseTrim(f.To.Country))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string ToLowercaseTrim(string s)
        {
            if (s != null)
            {
                return s!.ToLower().Trim();
            }

            return s;
        }

        public static void DeleteFlight(int id)
        {
            var flightIndex = 0;

            lock (_lock)
            {
                if (id <= _flights.Count && id >= 0)
                {
                    for (int i = 0; i < _flights.Count; i++)
                    {
                        if (id == _flights[i].Id)
                        {
                            flightIndex = i;
                        }
                    }

                    _flights.RemoveAt(flightIndex);
                }
            }
        }

        public static Airport[] SearchAirport(string search)
        {
            var airports = new List<Airport>();
            search = ToLowercaseTrim(search);

            lock (_lock)
            {
                foreach (var flight in _flights)
                {
                    if (ToLowercaseTrim(flight.To.Country).Contains(search) ||
                        ToLowercaseTrim(flight.To.AirportCode).Contains(search) ||
                        ToLowercaseTrim(flight.To.City).Contains(search))
                    {
                        airports.Add(flight.To);
                    }

                    if (ToLowercaseTrim(flight.From.Country).Contains(search) ||
                        ToLowercaseTrim(flight.From.AirportCode).Contains(search) ||
                        ToLowercaseTrim(flight.From.City).Contains(search))
                    {
                        airports.Add(flight.From);
                    }
                }
            }

            return airports.ToArray();
        }

        public static PageResult SearchFlight(SearchFlightsRequest search)
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
