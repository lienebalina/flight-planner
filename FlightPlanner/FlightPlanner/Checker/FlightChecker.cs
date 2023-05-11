using FlightPlanner.Models;
using System;

namespace FlightPlanner.Checker
{
    public static class FlightChecker
    {
        public static bool IsFlightValueNullOrEmpty(Flight flight)
        {
            if (flight.From == null || 
                (flight.From.AirportCode == null && flight.From.City == null && flight.From.Country == null) || 
                (flight.From.AirportCode == "" && flight.From.City == "" && flight.From.Country == "") ||
                flight.To == null ||
                (flight.To.AirportCode == null && flight.To.City == null && flight.To.Country == null) ||
                (flight.To.AirportCode == "" && flight.To.City == "" && flight.To.Country == "") ||
                flight.ArrivalTime == null || flight.Carrier == null ||
                flight.DepartureTime == null || flight.Carrier == "")
            {
                return true;
            }
            return false;
        }

        public static bool IsItTheSameAirport(Flight flight)
        {
            return flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim();
        }

        public static bool AreTheDepartureAndArrivalDateTimesValid(Flight flight)
        {
            var arrival = DateTime.Parse(flight.ArrivalTime);
            var departure = DateTime.Parse(flight.DepartureTime);

            if (DateTime.Compare(departure,arrival) > 0 ||
                DateTime.Compare(departure,arrival) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
