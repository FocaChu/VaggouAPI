using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ClientService : IClientService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ClientService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Client>> GetAllAsync() =>
            await _context.Clients.ToListAsync();


        public async Task<Client?> GetByIdAsync(Guid id) =>
            await _context.Clients.FindAsync(id);

        public async Task<Client> CreateAsync(ClientDto dto)
        {
            var created = _mapper.Map<Client>(dto);

            await _context.Clients.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }
    }
}
