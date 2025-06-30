namespace VaggouAPI
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public TimeOnly timeStart { get; set; }

        public TimeOnly timeEnd { get; set; }

        public Status Status { get; set; }

        public Guid VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public Guid ParkingSpotId { get; set; }

        public ParkingSpot ParkingSpot { get; set; }

        public Guid PaymentId { get; set; }

        public Payment Payment { get; set; }

    }
}
