namespace VaggouAPI
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetByParkingLotAsync(Guid parkingLotId);

        Task<Review> CreateAsync(CreateReviewRequestDto dto, Guid loggedInUserId);

        Task DeleteAsync(Guid reviewId, Guid loggedInUserId);
    }
}