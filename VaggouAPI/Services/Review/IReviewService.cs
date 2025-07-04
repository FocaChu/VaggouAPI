namespace VaggouAPI
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllAsync();

        Task<Review?> GetByIdAsync(Guid Id);

        Task<Review?> GetByClientAsync(Guid clientId);

        Task<Review?> GetByParkingLotAsync(Guid parkingLotId);

        Task<Review?> CreateAsync(ReviewDto review);

        Task<Review?> UpdateAsync(ReviewDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
