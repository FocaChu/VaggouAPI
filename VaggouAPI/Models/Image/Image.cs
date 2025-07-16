using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Image
    {
        public Guid Id { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public ImageType Type { get; set; }

        public Guid? ParkingLotId { get; set; }
        [JsonIgnore]
        public ParkingLot? ParkingLot { get; set; }

        public Guid? ClientId { get; set; }
        [JsonIgnore]
        public Client? Client { get; set; }

        public Guid? VehicleId { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }
    }
}
