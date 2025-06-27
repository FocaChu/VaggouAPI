using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class TestService<TEntity, TDto> : ITestService<TEntity, TDto>
    where TEntity : class
    {
        private readonly Db _context;
        private readonly IMapper _mapper;
        private readonly DbSet<TEntity> _dbSet;

        public TestService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            await _dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> UpdateAsync(Guid id, TDto dto)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return null;

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return false;

            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
