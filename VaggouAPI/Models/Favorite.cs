using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Favorite
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ClientId { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }

        public Guid ParkingLotId { get; set; }

        [JsonIgnore]
        public ParkingLot ParkingLot { get; set; }

    }
}
