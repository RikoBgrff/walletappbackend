using walletappbackend.Assets;

namespace walletappbackend.Services
{
    public class SeasonService
    {
        public DateTime GetSeasonStart(DateTime date)
        {
            // Determine the start of the season based on the date
            if (date.Month >= 3 && date.Month <= 5) return new DateTime(date.Year, 3, 1);  // Spring
            if (date.Month >= 6 && date.Month <= 8) return new DateTime(date.Year, 6, 1);  // Summer
            if (date.Month >= 9 && date.Month <= 11) return new DateTime(date.Year, 9, 1); // Autumn
            return new DateTime(date.Year, 12, 1);                                        // Winter
        }

        public Season GetCurrentSeason(DateTime date)
        {
            if (date.Month >= 3 && date.Month <= 5) return Season.Spring;
            if (date.Month >= 6 && date.Month <= 8) return Season.Summer;
            if (date.Month >= 9 && date.Month <= 11) return Season.Autumn;
            return Season.Winter;
        }
    }
}
