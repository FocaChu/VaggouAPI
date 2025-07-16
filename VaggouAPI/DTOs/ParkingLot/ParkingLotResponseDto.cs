namespace VaggouAPI
{
    public class ParkingLotResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }
        public Guid OwnerId { get; set; }
        public AddressResponseDto Address { get; set; }
        public ICollection<ParkingSpotSummaryResponseDto> ParkingSpots { get; set; } = new List<ParkingSpotSummaryResponseDto>();
        public ICollection<Guid> ImageIds { get; set; } = new List<Guid>();
    }
}
