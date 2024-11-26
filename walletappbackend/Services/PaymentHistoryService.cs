using Microsoft.EntityFrameworkCore;
using walletappbackend.Context;
using walletappbackend.Entities;

namespace walletappbackend.Services
{
    public class PaymentHistoryService
    {
        private readonly ApplicationDbContext _context;

        public PaymentHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentHistory?> GetCurrentPaymentHistoryAsync(Guid userId, int year, int month)
        {
            return await _context.PaymentHistories
                .FirstOrDefaultAsync(ph => ph.UserId == userId && ph.Year == year && ph.Month == month);
        }

        public async Task<bool> MarkPaymentAsPaidAsync(Guid userId)
        {
            var currentYear = DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;

            var paymentHistory = await GetCurrentPaymentHistoryAsync(userId, currentYear, currentMonth);

            if (paymentHistory != null && paymentHistory.IsPaid) return false;

            if (paymentHistory == null)
            {
                paymentHistory = new PaymentHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Year = currentYear,
                    Month = currentMonth,
                    IsPaid = true,
                    PaidDate = DateTime.UtcNow
                };
                await _context.PaymentHistories.AddAsync(paymentHistory);
            }
            else
            {
                paymentHistory.IsPaid = true;
                paymentHistory.PaidDate = DateTime.UtcNow;
                _context.PaymentHistories.Update(paymentHistory);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
