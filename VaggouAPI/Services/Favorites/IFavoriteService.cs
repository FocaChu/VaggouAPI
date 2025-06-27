using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetAllAsync();

        Task<Favorite?> GetByIdAsync(Guid id);

        Task<Favorite> CreateAsync([FromBody] FavoriteDto dto);

        Task<Favorite?> UpdateAsync([FromBody] FavoriteDto dto, Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
