namespace VaggouAPI
{
    public class ParkingLot
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Adress Address { get; set; }

        public Client Owner { get; set; }

        public ICollection<MonthlyReport> MonthlyReports { get; set; } = new List<MonthlyReport>();

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
