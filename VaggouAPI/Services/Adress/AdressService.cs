using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            var entity = _mapper.Map<Adress>(dto);

            _context.Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Adress?> UpdateAsync([FromBody] AdressDto dto, Guid id)
        {
            var entity = await _context.Adresses.FindAsync(id);

            if (entity == null) return null;

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();
            return entity;
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
