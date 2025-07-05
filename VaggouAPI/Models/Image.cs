namespace VaggouAPI
{
    public class Image
    {
        public Guid Id { get; set; }

        public string? FileName { get; set; }

        public byte[]? Content { get; set; }

        public string? ContentType { get; set; } 

        public ImageType Type { get; set; }

        public Guid ParkingLotId { get; set; }

        public ParkingLot? ParkingLot { get; set; }
    }
}
