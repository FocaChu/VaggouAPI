using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Payment
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public Status Status { get; set; }

        public Guid PaymentMethodId { get; set; }

        [JsonIgnore]
        public PaymentMethod? PaymentMethod { get; set; }

        [JsonIgnore]
        public Reservation? Reservation { get; set; }

    }
}
