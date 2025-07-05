using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Client
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string CPF { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        [JsonIgnore]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [JsonIgnore]
        public ICollection<ParkingLot> OwnedParkingLots { get; set; } = new List<ParkingLot>(); 
    }
}
