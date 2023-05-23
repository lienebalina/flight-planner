using FlightPlanner.Core.Models;
using FlightPlanner.Core.Validations;

namespace FlightPlanner.Services.Validations
{
    public class AirportValdator : IValidate
    {
        public bool IsValid(Flight flight)
        {
            return flight?.To != null && flight?.From != null;
        }
    }
}
