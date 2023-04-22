namespace FlightPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        /*public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Flight flight = (Flight)obj;
            return From.Equals(flight.From) && To.Equals(flight.To) &&
                   Carrier.Equals(flight.Carrier) &&
                   DepartureTime.Equals(flight.DepartureTime) &&
                   ArrivalTime.Equals(flight.ArrivalTime);
        }*/
    }
}
