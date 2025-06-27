namespace VaggouAPI
{
    public class VehicleDto
    {
        public string LicensePlate { get; set; }

        public bool IsPreRegistered { get; set; }

        public Guid VehicleModelId { get; set; }

        public Guid OwnerId { get; set; }
    }
}
