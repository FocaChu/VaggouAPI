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

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Adresses.AsNoTracking().ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(Guid id)
        {
            var address = await _context.Adresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id) 
                ?? throw new NotFoundException($"Address with Id: {id} not found.");

            return address;
        }
        public async Task<Address> CreateAsync(CreateAddressRequestDto dto)
        {
            var existingAddress = await _context.Adresses.FirstOrDefaultAsync(a =>
                a.Street == dto.Street &&
                a.Number == dto.Number &&
                a.ZipCode == dto.ZipCode &&
                a.City == dto.City &&
                a.State == dto.State);

            if (existingAddress != null)
            {
                throw new BusinessException("An identical address already exists.");
            }

            var addressEntity = _mapper.Map<Address>(dto);

            await _context.Adresses.AddAsync(addressEntity);
            await _context.SaveChangesAsync();

            return addressEntity;
        }

        public async Task<Address> UpdateAsync(Guid id, UpdateAddressRequestDto dto)
        {
            var addressEntity = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Address to update not found.");

            _mapper.Map(dto, addressEntity);

            await _context.SaveChangesAsync();

            return addressEntity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var addressEntity = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Address to delete not found.");

            _context.Adresses.Remove(addressEntity);
            await _context.SaveChangesAsync();
        }
    }
}