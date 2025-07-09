using Repository.Data;
using Repository.Models.Entities;
using Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class InvalidTokenRepository : IInvalidTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public InvalidTokenRepository(ApplicationDbContext context) => _context = context;

        public async Task<InvalidatedToken> GetByIdAsync(string id)
        {
            try
            {
                return await _context.InvalidatedTokens.FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting invalidated token by id: {ex.Message}");
                throw;
            }
        }

        public async Task CreateAsync(InvalidatedToken token)
        {
            try
            {
                _context.InvalidatedTokens.Add(token);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating invalidated token: {ex.Message}");
                throw;
            }
        }
    }
}
