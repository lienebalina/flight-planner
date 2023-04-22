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
            flight.Id = _id++;
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
                if (f == flight)
                {
                        return true;
                }
            }

            return false;
        }
    }
}
