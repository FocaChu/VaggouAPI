namespace VaggouAPI
{
    public interface IIAChatService
    {
        Task<string> SendMensageAsync(string mensagem);
    }
}
