namespace VaggouAPI
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }

        public int Score { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid ParkingLotId { get; set; }

        public string ClientName { get; set; } 
    }
}
