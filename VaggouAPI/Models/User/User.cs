namespace VaggouAPI
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public Client Client { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
