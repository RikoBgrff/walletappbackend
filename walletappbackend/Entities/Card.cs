namespace walletappbackend.Entities
{
    public class Card
    {
        public Guid Id { get; set; }  
        public Guid UserId { get; set; }  
        public decimal Balance { get; set; }  
        public decimal Limit { get; set; } = 1500;  
        public decimal Available => Limit - Balance;  
        public virtual User User { get; set; } = null!;  
    }

}
