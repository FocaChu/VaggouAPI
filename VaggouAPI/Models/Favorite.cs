namespace VaggouAPI
{
    public class Favorite
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Client Client { get; set; }

        public ParkingLot ParkingLot { get; set; }

    }
}
