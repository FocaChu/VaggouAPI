using System.ComponentModel.DataAnnotations;

namespace VaggouAPI
{
    public class MonthlyReportData
    {
        [Key]
        public Guid Id { get; set; } 

        public int TotalReservations { get; set; }

        public int TotalCancelations { get; set; }

        public double TotalRevenue { get; set; }

        public double ScoreChange { get; set; }

        public List<string> PeakHours { get; set; } = new();

        public Guid MonthlyReportId { get; set; }

        public MonthlyReport MonthlyReport { get; set; }
    }

}
