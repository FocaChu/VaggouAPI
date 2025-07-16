namespace VaggouAPI
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public ClientProfileResponseDto UserProfile { get; set; } 
    }
}
