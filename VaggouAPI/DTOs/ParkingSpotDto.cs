namespace VaggouAPI
{
    public class ParkingSpotDto
    {
        public string SpotIdentifier { get; set; }

        public double PricePerHour { get; set; }

        public bool IsCovered { get; set; }

        public bool IsPCDSpace { get; set; }

        public bool IsDisabled { get; set; }

        public Size Size { get; set; }

        public Guid ParkingLotId { get; set; }
    }
}
