using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class PaymentService : IPaymentService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public PaymentService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync() =>
            await IncludeAll().ToListAsync();

        public async Task<Payment?> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new NotFoundException("Payment not found.");

        public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(Guid paymentMethodId) =>
            await IncludeAll()
                .Where(p => p.PaymentMethodId == paymentMethodId)
                .ToListAsync();

        public async Task<IEnumerable<Payment>> GetByMonthAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            return await IncludeAll()
                .Where(p => p.PaidAt >= startDate && p.PaidAt < endDate)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(PaymentDto dto)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(dto.PaymentMethodId)
                ?? throw new BusinessException("PaymentMethod not found.");

            var created = _mapper.Map<Payment>(dto);
            created.PaymentMethod = paymentMethod;

            await _context.Payments.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Payment?> UpdateAsync(PaymentDto dto, Guid id)
        {
            var updated = await IncludeAll().FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new BusinessException("Payment not found.");

            var reservation = await _context.Reservations.FirstOrDefaultAsync(e => e.Id == dto.ReservationId)
                ?? throw new BusinessException("Reservation not found.");

            _mapper.Map(dto, updated);
            updated.Reservation = reservation;

            _context.Payments.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await IncludeAll().FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new BusinessException("Payment not found.");

            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Payment> IncludeAll() =>
            _context.Payments
                .Include(pl => pl.Reservation)
                .Include(pl => pl.PaymentMethod);
    }
}
