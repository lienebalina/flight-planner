using FlightPlanner.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Data
{
    public interface IFlightPlannerContext
    {
        DbSet<T> Set<T>() where T : class;

        EntityEntry<T> Entry<T>(T entry) where T : class;

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }

        public int SaveChanges();
    }
}
