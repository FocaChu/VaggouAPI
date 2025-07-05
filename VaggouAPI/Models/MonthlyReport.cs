using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class MonthlyReport
    {
        public Guid Id { get; set; }

        public string? AiAnalysis { get; set; }

        public int TotalReservations { get; set; }

        public int TotalCancelations { get; set; }

        public double TotalRevenue { get; set; }

        public double ScoreChange { get; set; }

        public List<string> PeakHours { get; set; } = new();

        public Guid ParkingLotId { get; set; }

        [JsonIgnore]
        public ParkingLot? ParkingLot { get; set; }
    }
}
