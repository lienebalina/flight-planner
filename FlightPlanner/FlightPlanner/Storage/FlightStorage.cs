using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 1;
        
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
            if (flight != null)
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
    }
}
