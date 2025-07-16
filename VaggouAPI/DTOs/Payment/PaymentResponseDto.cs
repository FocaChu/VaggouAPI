namespace VaggouAPI
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public Status Status { get; set; }

        public Guid ReservationId { get; set; }

        public PaymentMethodResponseDto PaymentMethod { get; set; }
    }
}
