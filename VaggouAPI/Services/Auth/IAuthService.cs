namespace VaggouAPI
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
