namespace VaggouAPI
{
    public class FavoriteResponseDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public ParkingLotResponseDto ParkingLot { get; set; }
    }
}
