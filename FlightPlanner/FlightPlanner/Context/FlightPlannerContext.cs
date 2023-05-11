using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Context
{
    public class FlightPlannerContext : DbContext
    {
        public FlightPlannerContext(DbContextOptions options) : base(options) { }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
    }
}
