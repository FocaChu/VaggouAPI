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

        public async Task<IEnumerable<Review>> GetAllAsync() =>
            await _context.Reviews.ToListAsync();

        public async Task<Review?> GetByIdAsync(Guid Id) =>
            await _context.Reviews.FindAsync(Id)
                ?? throw new NotFoundException("Review not found.");

        public async Task<Review?> GetByClientAsync(Guid clientId) =>
            await _context.Reviews
                .Where(r => r.ClientId == clientId)
                .FirstOrDefaultAsync();

        public async Task<Review?> GetByParkingLotAsync(Guid parkingLotId) =>
            await _context.Reviews
                .Where(r => r.ParkingLotId == parkingLotId)
                .FirstOrDefaultAsync();

        public async Task<Review?> CreateAsync(ReviewDto review)
        {
            var client = await _context.Clients.FindAsync(review.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var parkingLot = await _context.ParkingLots.FindAsync(review.ParkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            var created = _mapper.Map<Review>(review);
            created.Client = client;
            created.ParkingLot = parkingLot;

            await _context.Reviews.AddAsync(created);
            await _context.SaveChangesAsync();

            return created;
        }

        public async Task<Review?> UpdateAsync(ReviewDto dto, Guid id)
        {
            var updated = await _context.Reviews.FindAsync(id)
                ?? throw new NotFoundException("Review not found.");

            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            _mapper.Map(dto, updated);
            updated.Client = client;
            updated.ParkingLot = parkingLot;

            _context.Reviews.Update(updated);
            await _context.SaveChangesAsync();

            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Reviews.FindAsync(id)
                ?? throw new NotFoundException("Review not found.");

            _context.Reviews.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
