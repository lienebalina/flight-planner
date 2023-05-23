using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Search
{
    public interface ISearch
    {
        Airport[] SearchAirports(string search);
        PageResult SearchFlight(SearchFlightsRequest search);
    }
}
