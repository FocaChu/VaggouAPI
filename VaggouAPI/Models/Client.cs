namespace VaggouAPI
{
    public class Client
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
