using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ReviewService : IReviewService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ReviewService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Review>> GetByParkingLotAsync(Guid parkingLotId)
        {
            return await _context.Reviews
                .Where(r => r.ParkingLotId == parkingLotId)
                .Include(r => r.Client) 
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review> CreateAsync(CreateReviewRequestDto dto, Guid loggedInUserId)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            if (parkingLot.OwnerId == loggedInUserId)
            {
                throw new BusinessException("You cannot review your own parking lot.");
            }

            var existingReview = await _context.Reviews.AnyAsync(r =>
                r.ClientId == loggedInUserId && r.ParkingLotId == dto.ParkingLotId);
            if (existingReview)
            {
                throw new BusinessException("You have already submitted a review for this parking lot.");
            }

            var hasCompletedReservation = await _context.Reservations.AnyAsync(r =>
                r.ClientId == loggedInUserId &&
                r.ParkingSpot.ParkingLotId == dto.ParkingLotId &&
                r.Status == Status.Success); 

            if (!hasCompletedReservation)
            {
                throw new BusinessException("You can only review a parking lot after completing a reservation.");
            }

            // Mapeamento e Criação
            var reviewEntity = _mapper.Map<Review>(dto);
            reviewEntity.ClientId = loggedInUserId; 
            reviewEntity.CreatedAt = DateTime.UtcNow;

            await _context.Reviews.AddAsync(reviewEntity);
            await _context.SaveChangesAsync();

            return reviewEntity;
        }

        public async Task DeleteAsync(Guid reviewId, Guid loggedInUserId)
        {
            var reviewEntity = await _context.Reviews.FindAsync(reviewId);

            if (reviewEntity == null)
            {
                return;
            }
            if (reviewEntity.ClientId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this review.");
            }

            _context.Reviews.Remove(reviewEntity);
            await _context.SaveChangesAsync();
        }
    }
}