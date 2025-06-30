namespace VaggouAPI
{
    public class Client
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
