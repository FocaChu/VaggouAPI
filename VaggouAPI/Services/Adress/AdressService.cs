using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class AdressService : IAdressService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public AdressService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Adress>> GetAllAsync() =>
            await _context.Adresses.ToListAsync();
        

        public async Task<Adress?> GetByIdAsync(Guid id) =>
            await _context.Adresses.FindAsync(id);
        

        public async Task<Adress> CreateAsync(AdressDto dto)
        {
            var created = _mapper.Map<Adress>(dto);

            await _context.Adresses.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Adress?> UpdateAsync(AdressDto dto, Guid id)
        {
            var updated = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Endereço não encontrado para deleção.");

            _mapper.Map(dto, updated);

            _context.Adresses.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Adresses.FindAsync(id)
                ?? throw new NotFoundException("Endereço não encontrado para deleção.");

            _context.Adresses.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}
