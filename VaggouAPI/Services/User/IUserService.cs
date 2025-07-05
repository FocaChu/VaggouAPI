namespace VaggouAPI
{
    public interface IUserService
    {
        Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync();

        Task<UserDetailDto> GetUserByIdAsync(Guid id);

        Task UpdateUserRolesAsync(Guid id, UpdateUserRolesDto dto);
    }
}
