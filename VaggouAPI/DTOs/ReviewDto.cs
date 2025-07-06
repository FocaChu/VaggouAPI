namespace VaggouAPI
{
    public class ReviewDto
    {
        public int Score { get; set; }

        public string Comment { get; set; } 

        public Guid ClientId { get; set; }

        public Guid ParkingLotId { get; set; }
    }
}
