namespace VaggouAPI
{
    public class VehicleModelDto
    {
        public string Brand { get; set; }

        public string ModelName { get; set; }

        public string Year { get; set; }

        public VehicleType VehicleType { get; set; }

        public FuelType FuelType { get; set; }
    }
}
