namespace VaggouAPI
{
    public class ParkingSpot
    {
        public Guid Id { get; set; }

        public string SpotIdentifier { get; set; }

        public double PricePerHour { get; set; }

        public Size Size { get; set; }

        public ParkingLot ParkingLot { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
