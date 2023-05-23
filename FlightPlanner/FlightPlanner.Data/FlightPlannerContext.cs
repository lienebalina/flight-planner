using FlightPlanner.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Data
{
    public class FlightPlannerContext : DbContext, IFlightPlannerContext
    {
        public FlightPlannerContext(DbContextOptions options) : base(options) { }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
    }
}
