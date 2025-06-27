namespace VaggouAPI.DTOs
{
    public class ClientDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints { get; set; }
        public Guid UserId { get; set; }
    }
}
