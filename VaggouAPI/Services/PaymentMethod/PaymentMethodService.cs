using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public PaymentMethodService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod?> GetByIdAsync(Guid id)
        {
            return await _context.PaymentMethods.FindAsync(id);
        }

        public async Task<PaymentMethod> CreateAsync(PaymentMethodDto dto)
        {
            var created = _mapper.Map<PaymentMethod>(dto);

            await _context.PaymentMethods.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<PaymentMethod?> UpdateAsync(PaymentMethodDto dto, Guid id)
        {
            var updated = await _context.PaymentMethods.FindAsync(id);

            if (updated == null) return null;

            _mapper.Map(dto, updated);

            _context.PaymentMethods.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.PaymentMethods.FindAsync(id);

            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
