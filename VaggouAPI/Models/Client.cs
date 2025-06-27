namespace VaggouAPI.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints { get; set; } //pontos de fidelidade

        public User User { get; set; }
    }
}
