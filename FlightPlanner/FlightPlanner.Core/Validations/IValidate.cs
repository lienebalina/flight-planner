using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Validations
{
    public interface IValidate
    {
        bool IsValid(Flight flight);
    }
}
