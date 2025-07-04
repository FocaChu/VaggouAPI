using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Address
    {
        public Guid Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string? ZipCode { get; set; }

        public string Country { get; set; }

        public string Complement { get; set; }

        public int Longitude { get; set; }

        public int Latitude { get; set; }

        [JsonIgnore]
        public ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();
    }
}
