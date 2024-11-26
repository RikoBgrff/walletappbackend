namespace walletappbackend.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }  
        public Guid UserId { get; set; } 
        public string Type { get; set; } = null!; 
        public decimal Amount { get; set; }  
        public string Name { get; set; } = null!;  
        public string Description { get; set; } = null!;  
        public DateTime Date { get; set; }  
        public bool IsPending { get; set; }  
        public string? AuthorizedUser { get; set; }  
        public virtual User User { get; set; } = null!;  
    }

}
