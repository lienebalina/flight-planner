using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        
        public static Flight GetFlight(int id)
        {
            return _flights.SingleOrDefault(flight => flight.Id == id);
        }

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = _flights.Count;
            _flights.Add(flight);
            
            return flight;
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static bool CheckSameFlights(Flight flight)
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
}
