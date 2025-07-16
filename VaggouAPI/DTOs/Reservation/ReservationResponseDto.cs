namespace VaggouAPI
{
    public class ReservationResponseDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public TimeOnly TimeStart { get; set; }

        public TimeOnly TimeEnd { get; set; }

        public Status Status { get; set; }

        public VehicleSummaryResponseDto Vehicle { get; set; }

        public ClientSummaryResponseDto Client { get; set; }

        public ParkingSpotSummaryResponseDto ParkingSpot { get; set; }
    }
}
