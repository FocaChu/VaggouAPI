using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public string LicensePlate { get; set; }

        public bool IsPreRegistered { get; set; }

        public Guid VehicleModelId { get; set; }

        [JsonIgnore]
        public VehicleModel VehicleModel { get; set; }

        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public Client Owner { get; set; }

        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
