using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VaggouAPI;

namespace VaggouAP
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

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await IncludeAll().ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await IncludeAll().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(Guid paymentMethodId)
        {
            return await IncludeAll()
                .Where(p => p.PaymentMethodId == paymentMethodId)
                .ToListAsync();
        }

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
            var created = _mapper.Map<Payment>(dto);

            var paymentMethod = await _context.PaymentMethods.FindAsync(dto.PaymentMethodId);
            if (paymentMethod == null)
                throw new BusinessException("PaymentMethod not found.");
            else
            {
                created.PaymentMethod = paymentMethod;
                created.PaymentMethodId = paymentMethod.Id;
            }

            await _context.Payments.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Payment?> UpdateAsync(PaymentDto dto, Guid id)
        {
            var updated = await IncludeAll().FirstOrDefaultAsync(e => e.Id == id);

            if (updated == null) return null;

            _mapper.Map(dto, updated);

            _context.Payments.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await IncludeAll().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<Payment> IncludeAll()
        {
            return _context.Payments
                .Include(pl => pl.Reservation)
                .Include(pl => pl.PaymentMethod);
        }
    }
}
