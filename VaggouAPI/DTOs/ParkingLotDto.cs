namespace VaggouAPI
{
    public class ParkingLotDto
    {
        public string Name { get; set; }

        public Guid AddressId { get; set; }

        public Guid OwnerId { get; set; }
    }
}
