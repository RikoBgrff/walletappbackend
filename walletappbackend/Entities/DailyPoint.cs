namespace walletappbackend.Entities
{
    public class DailyPoint
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int Points { get; set; }
        public virtual User User { get; set; } = null!;
    }

}
