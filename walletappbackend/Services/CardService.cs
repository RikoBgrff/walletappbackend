using Microsoft.EntityFrameworkCore;
using walletappbackend.Context;
using walletappbackend.Entities;

namespace walletappbackend.Services
{
    public class CardService
    {
        private readonly ApplicationDbContext _context;

        public CardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Card?> GetCardByUserAsync(Guid userId)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
