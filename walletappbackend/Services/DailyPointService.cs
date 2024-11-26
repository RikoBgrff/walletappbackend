using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using walletappbackend.Context;
using walletappbackend.Entities;

namespace walletappbackend.Services
{
    public class DailyPointsService
    {
        private readonly ApplicationDbContext _context;
        private readonly SeasonService _seasonService;

        public DailyPointsService(ApplicationDbContext context, SeasonService seasonService)
        {
            _context = context;
            _seasonService = seasonService;
        }

        public async Task<int> CalculateDailyPointsAsync(DateTime date, Guid userId)
        {
            // Check if points for today already exist
            var existingPoints = await _context.DailyPoints
                .FirstOrDefaultAsync(dp => dp.UserId == userId && dp.Date.Date == date.Date);

            if (existingPoints != null)
            {
                // Points already calculated for today
                return existingPoints.Points;
            }

            // Determine the season start date
            var seasonStart = _seasonService.GetSeasonStart(date);
            var dayOfSeason = (date - seasonStart).Days + 1;

            int points;

            if (dayOfSeason == 1)
            {
                points = 2;
            }
            else if (dayOfSeason == 2)
            {
                points = 3;
            }
            else
            {
                // Fetch the previous two days' points
                var previousDayPoints = await _context.DailyPoints
                    .Where(dp => dp.UserId == userId && dp.Date.Date == date.AddDays(-1).Date)
                    .Select(dp => dp.Points)
                    .FirstOrDefaultAsync();

                var twoDaysAgoPoints = await _context.DailyPoints
                    .Where(dp => dp.UserId == userId && dp.Date.Date == date.AddDays(-2).Date)
                    .Select(dp => dp.Points)
                    .FirstOrDefaultAsync();

                points = (int)Math.Round(twoDaysAgoPoints * 1.0 + previousDayPoints * 0.6);
            }

            // Cap points if above 1000
            if (points > 1000)
            {
                points = points / 1000 * 1000;
            }

            // Persist calculated points to the database
            var dailyPoint = new DailyPoint
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = date,
                Points = points
            };

            await _context.DailyPoints.AddAsync(dailyPoint);
            await _context.SaveChangesAsync();

            return points;
        }
    }
}
