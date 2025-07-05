namespace VaggouAPI
{
    public interface IClientService
    {
        Task<ClientProfileDto> GetMyProfileAsync(Guid loggedInUserId);

        Task<ClientProfileDto> UpdateMyProfileAsync(Guid loggedInUserId, UpdateClientProfileDto dto);
    }
}
