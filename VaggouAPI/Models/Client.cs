using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Client
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        [JsonIgnore]
        public ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
