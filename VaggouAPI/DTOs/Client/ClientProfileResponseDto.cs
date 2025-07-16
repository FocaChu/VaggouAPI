namespace VaggouAPI
{
    public class ClientProfileResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; } 

        public string Phone { get; set; }

        public string CPF { get; set; }

        public Guid? ProfileImageId { get; set; }
    }
}
