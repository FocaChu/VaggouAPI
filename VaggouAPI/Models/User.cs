namespace VaggouAPI
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public Client? Client { get; set; }
    }
}
