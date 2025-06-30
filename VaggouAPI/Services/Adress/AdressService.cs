using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IEnumerable<Adress>> GetAllAsync()
        {
            return await _context.Adresses.ToListAsync();
        }

        public async Task<Adress?> GetByIdAsync(Guid id)
        {
            return await _context.Adresses.FindAsync(id);
        }

        public async Task<Adress> CreateAsync([FromBody] AdressDto dto)
        {
            var created = _mapper.Map<Adress>(dto);

            await _context.Adresses.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Adress?> UpdateAsync([FromBody] AdressDto dto, Guid id)
        {
            var updated = await _context.Adresses.FindAsync(id);

            if (updated == null) return null;

            _mapper.Map(dto, updated);

            _context.Adresses.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Adresses.FindAsync(id);

            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
