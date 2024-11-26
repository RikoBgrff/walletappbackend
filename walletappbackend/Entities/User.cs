using System.Security.Principal;

namespace walletappbackend.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!; 
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); 
    }

}
