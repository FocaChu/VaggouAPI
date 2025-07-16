namespace VaggouAPI
{
    public interface IClientService
    {
        Task<Client> GetMyProfileAsync(Guid loggedInUserId);
        Task<Client> UpdateMyProfileAsync(Guid loggedInUserId, UpdateClientProfileRequestDto dto);
    }
}
