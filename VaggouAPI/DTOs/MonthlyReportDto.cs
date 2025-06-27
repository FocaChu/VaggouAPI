namespace VaggouAPI
{
    public class MonthlyReportDto
    {
        public int TotalReservations { get; set; }

        public int TotalCancellations { get; set; }

        public int TotalReveneue { get; set; }

        public string PeakHour { get; set; }

        public string AiAnalysis { get; set; }

        public Guid ParkingLotId { get; set; }
    }
}
