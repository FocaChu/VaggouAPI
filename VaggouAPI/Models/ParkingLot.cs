using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class ParkingLot
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public double Score { get; set; }

        public Guid AdressId { get; set; }

        public Address? Address { get; set; }

        public Guid OwnerId { get; set; }

        public Client? Owner { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [JsonIgnore]
        public ICollection<MonthlyReportService> MonthlyReports { get; set; } = new List<MonthlyReportService>();

        [JsonIgnore]
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
