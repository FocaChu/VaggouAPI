namespace VaggouAPI
{
    public class Favorite
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public Guid ParkingLotId { get; set; }

        public ParkingLot ParkingLot { get; set; }

    }
}
