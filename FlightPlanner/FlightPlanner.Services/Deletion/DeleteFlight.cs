using FlightPlanner.Core.Deletion;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services.Deletion
{
    public class DeleteFlight : IFlightDelete
    {
        private readonly IFlightService _flightService;
        private readonly IFlightPlannerContext _context;

        public DeleteFlight(IFlightService flightService, IFlightPlannerContext context)
        {
            _flightService = flightService;
            _context = context;
        }
        public void Delete(int id)
        {
            var flightToDelete = _flightService.GetFullFlight(id);

            if (flightToDelete != null)
            {
                _context.Flights.Remove(flightToDelete);
                _context.Airports.Remove(flightToDelete.From);
                _context.Airports.Remove(flightToDelete.To);
                _context.SaveChanges();
            }
        }
    }
}
