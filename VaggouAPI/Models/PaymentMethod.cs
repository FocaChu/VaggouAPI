namespace VaggouAPI
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
