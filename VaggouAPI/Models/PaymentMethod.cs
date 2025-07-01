using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
