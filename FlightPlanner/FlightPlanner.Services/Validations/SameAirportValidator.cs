using System;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Validations;

namespace FlightPlanner.Services.Validations
{
    public class SameAirportValidator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            return !string.Equals(flight?.From?.AirportCode.Trim(), flight?.To?.AirportCode.Trim(),
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
