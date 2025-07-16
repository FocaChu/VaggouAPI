namespace VaggouAPI
{
    public class CreateReservationRequestDto
    {
        public DateTime Date { get; set; }

        public TimeOnly TimeStart { get; set; }

        public TimeOnly TimeEnd { get; set; }

        public Guid VehicleId { get; set; }

        public Guid ParkingSpotId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
