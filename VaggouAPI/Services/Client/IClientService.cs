namespace VaggouAPI
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllAsync();

        Task<Client?> GetByIdAsync(Guid id);

        Task<Client> CreateAsync(ClientDto dto);
    }
}
