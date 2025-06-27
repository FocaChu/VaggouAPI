namespace VaggouAPI
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public Status Status { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}
