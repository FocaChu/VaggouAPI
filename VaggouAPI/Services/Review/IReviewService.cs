namespace VaggouAPI
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllAsync();

        Task<Review?> GetByIdAsync(Guid Id);
    }
}
