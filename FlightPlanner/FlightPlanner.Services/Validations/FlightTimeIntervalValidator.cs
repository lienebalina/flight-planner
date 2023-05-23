using System;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Validations;

namespace FlightPlanner.Services.Validations
{
    public class FlightTimeIntervalValidator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            var arrival = DateTime.Parse(flight?.ArrivalTime);
            var departure = DateTime.Parse(flight?.DepartureTime);

            return arrival > departure;
        }
    }
}
