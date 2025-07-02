using System.Runtime.CompilerServices;

namespace VaggouAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  Role Role{ get; set; }
        public string TokenAcess { get; set; }
        //public string RefreshToken { get; set; }
    }
}
