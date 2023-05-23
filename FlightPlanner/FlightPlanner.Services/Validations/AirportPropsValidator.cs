using FlightPlanner.Core.Models;
using FlightPlanner.Core.Validations;

namespace FlightPlanner.Services.Validations
{
    public class AirportPropsValidator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            return !string.IsNullOrEmpty(flight?.To?.AirportCode) && 
                   !string.IsNullOrEmpty(flight?.To?.City) && 
                   !string.IsNullOrEmpty(flight?.To?.Country) &&
                   !string.IsNullOrEmpty(flight?.From?.AirportCode) &&
                   !string.IsNullOrEmpty(flight?.From?.City) &&
                   !string.IsNullOrEmpty(flight?.From?.Country);
        }
    }
}
