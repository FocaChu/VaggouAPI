namespace VaggouAPI
{
    public class ReservationDto
    {
        public DateTime Date { get; set; }

        public TimeOnly timeStart { get; set; }

        public TimeOnly timeEnd { get; set; }

        public Status Status { get; set; }

        public Guid VehicleId { get; set; }

        public Guid ClientId { get; set; }

        public Guid ParkingSpotId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
