namespace VaggouAPI
{
    public class ParkingSpotDto
    {
        public string SpotIdentifier { get; set; }

        public double PricePerHour { get; set; }

        public Size Size { get; set; }

        public Guid ParkingLotId { get; set; }
    }
}
