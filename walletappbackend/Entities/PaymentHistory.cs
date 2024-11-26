namespace walletappbackend.Entities
{
    public class PaymentHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; } // Use month number (1-12)
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
