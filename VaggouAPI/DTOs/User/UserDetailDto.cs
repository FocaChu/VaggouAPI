namespace VaggouAPI
{
    public class UserDetailDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; } 

        public string Phone { get; set; }   

        public ICollection<string> Roles { get; set; } = new List<string>();

        public int OwnedParkingLotsCount { get; set; } 
    }
}
