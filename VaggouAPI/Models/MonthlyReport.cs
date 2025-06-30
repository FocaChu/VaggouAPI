namespace VaggouAPI
{
    public class MonthlyReport
    {
        public Guid Id { get; set; }
        
        public int TotalReservations { get; set; }

        public int TotalCancellations { get; set; }

        public int TotalReveneue { get; set; }

        public string PeakHour { get; set; }

        public string AiAnalysis { get; set; }

        public DateTime GeneretadAt { get; set; } = DateTime.Now;

        public Guid ParkingLotId { get; set; }

        public ParkingLot ParkingLot { get; set; }
    }
}
