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

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync() =>
            await _context.PaymentMethods.AsNoTracking().ToListAsync();

        public async Task<PaymentMethod> GetByIdAsync(Guid id) =>
            await _context.PaymentMethods.AsNoTracking().FirstOrDefaultAsync(pm => pm.Id == id)
                ?? throw new NotFoundException("Payment method not found.");

        public async Task<PaymentMethod> CreateAsync(CreatePaymentMethodRequestDto dto)
        {
            var nameExists = await _context.PaymentMethods.AnyAsync(pm => pm.Name == dto.Name);
            if (nameExists)
            {
                throw new BusinessException($"A payment method with the name '{dto.Name}' already exists.");
            }

            var entity = _mapper.Map<PaymentMethod>(dto);
            await _context.PaymentMethods.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PaymentMethod> UpdateAsync(Guid id, CreatePaymentMethodRequestDto dto)
        {
            var entity = await _context.PaymentMethods.FindAsync(id)
                ?? throw new NotFoundException("Payment method not found.");

            var nameExists = await _context.PaymentMethods.AnyAsync(pm => pm.Name == dto.Name && pm.Id != id);
            if (nameExists)
            {
                throw new BusinessException($"A payment method with the name '{dto.Name}' already exists.");
            }

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var isInUse = await _context.Payments.AnyAsync(p => p.PaymentMethodId == id);
            if (isInUse)
            {
                throw new BusinessException("This payment method cannot be deleted as it is associated with existing payments.");
            }

            var entity = await _context.PaymentMethods.FindAsync(id)
                ?? throw new NotFoundException("Payment method not found.");

            _context.PaymentMethods.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}