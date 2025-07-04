using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class MonthlyReport
    {
        public Guid Id { get; set; }

        public string AiAnalysis { get; set; }

        public MonthlyReportData MonthlyReportData { get; set; }

        public Guid ParkingLotId { get; set; }

        [JsonIgnore]
        public ParkingLot ParkingLot { get; set; }
    }
}
