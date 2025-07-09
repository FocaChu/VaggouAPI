using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class AddressService : IAddressService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public AddressService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Address>> GetAllAsync() =>
            await _context.Adresses.ToListAsync();
        

        public async Task<Address?> GetByIdAsync(Guid id) =>
            await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException($"Address with Id: {id} not found.");


        public async Task<Address> CreateAsync(AddressDto dto)
        {
            var created = _mapper.Map<Address>(dto);

            await _context.Adresses.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Address?> UpdateAsync(AddressDto dto, Guid id)
        {
            var updated = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Address not found.");

            _mapper.Map(dto, updated);

            _context.Adresses.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Address not found.");

            _context.Adresses.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}
