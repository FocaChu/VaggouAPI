namespace VaggouAPI
{
    public class Payment
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public Status Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; } 
    }
}
