namespace VaggouAPI
{
    public class InitiatePaymentRequestDto
    {
        public Guid ReservationId { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}
