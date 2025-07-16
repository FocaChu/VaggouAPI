namespace VaggouAPI
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetMyFavoritesAsync(Guid loggedInUserId);

        Task<Favorite> CreateAsync(Guid loggedInUserId, CreateFavoriteRequestDto dto);

        Task DeleteAsync(Guid favoriteId, Guid loggedInUserId);
    }
}

