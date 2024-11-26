using Microsoft.EntityFrameworkCore;
using walletappbackend.Context;
using walletappbackend.Entities;

namespace walletappbackend.Services
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetTransactionDetailAsync(Guid transactionId)
        {
            return await _context.Transactions
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Transactions.OrderByDescending(t => t.Date).Take(10) ?? Enumerable.Empty<Transaction>();
        }
    }
}
