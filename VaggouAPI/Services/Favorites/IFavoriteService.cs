namespace VaggouAPI
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetAllAsync();

        Task<Favorite?> GetByIdAsync(Guid id);

        Task<IEnumerable<Favorite>> GetByClientIdAsync(Guid clientId);

        Task<Favorite> CreateAsync(FavoriteDto dto);

        Task<Favorite?> UpdateAsync(FavoriteDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
