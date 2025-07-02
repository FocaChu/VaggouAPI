using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class VehicleModel
    {
        public Guid Id { get; set; }

        public string Brand { get; set; }

        public string ModelName { get; set; }

        public string Year { get; set; }

        public VehicleType VehicleType { get; set; }

        public FuelType FuelType { get; set; }

        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
