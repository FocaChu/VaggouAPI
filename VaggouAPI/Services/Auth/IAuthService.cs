namespace VaggouAPI
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto registerDto);

        Task<AuthResponseDto> Login(LoginRequestDto loginDto);
    }
}
