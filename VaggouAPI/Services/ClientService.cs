using System.Xml;
using Microsoft.EntityFrameworkCore;
using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Services
{
    public class ClientService
    {
        private readonly Db _context;

        public ClientService(Db _context)
        {
            this._context = _context;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _context.Clients.Include(c=> c.User).ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c=> c.Id == id);

            if (client == null) throw new BusinessException("Client Not Found");

            return client;
        }

        //procura se tem algum user no banco com mesmo id pra nao criar dois cliente em um user
        public async Task<Client> CreateAsync(ClientDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) throw new BusinessException("User Not Found");

            var client2 = await _context.Clients
                .Include(c =>c.User)
                .FirstOrDefaultAsync(c => c.User.Id == dto.UserId);
            if (client2 != null) throw new BusinessException("User is already in use");

            var client = new Client()
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Phone = dto.Phone,
                LoyaltyPoints = dto.LoyaltyPoints,
                User = user,
            };
            
            await _context.Clients.AddAsync(client);

            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateAsync(Guid id, ClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) throw new BusinessException("Client Not Found");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) throw new BusinessException("User Not Found");

            client.FullName = dto.FullName;
            client.Phone = dto.Phone;
            client.LoyaltyPoints = dto.LoyaltyPoints;
            client.User = user;

            await _context.SaveChangesAsync();
            return client;
        }

        //fazer ele deletar a conta de user quando deletar o client
        public async Task<bool> DeleteAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) throw new BusinessException("Client Not Found");

            _context.Clients.Remove(client);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
