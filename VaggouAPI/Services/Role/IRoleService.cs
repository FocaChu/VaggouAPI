namespace VaggouAPI
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();

        Task<Role> GetByIdAsync(Guid id);

        Task<Role> CreateAsync(CreateRoleRequestDto dto);

        Task DeleteAsync(Guid id);
    }
}