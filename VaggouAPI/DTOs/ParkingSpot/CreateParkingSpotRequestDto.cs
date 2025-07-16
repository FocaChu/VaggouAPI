namespace VaggouAPI
{
    public class CreateParkingSpotRequestDto
    {
        public string SpotIdentifier { get; set; }

        public double PricePerHour { get; set; }

        public bool IsCovered { get; set; }

        public bool IsPCDSpace { get; set; }

        public Size Size { get; set; }
    }
}
