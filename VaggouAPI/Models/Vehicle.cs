namespace VaggouAPI
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public string LicensePlate { get; set; }

        public bool IsPreRegistered { get; set; }

        public VehicleModel VehicleModel { get; set; }

        public Client Owner { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
