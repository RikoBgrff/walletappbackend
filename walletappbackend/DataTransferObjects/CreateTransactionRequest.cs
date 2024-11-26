using System.ComponentModel.DataAnnotations;

namespace walletappbackend.DataTransferObjects
{
    public class CreateTransactionRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType))]
        public string Type { get; set; } = null!; // Must match enum values like "Payment" or "Credit"

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = null!;

        public bool IsPending { get; set; }

        [StringLength(100, ErrorMessage = "AuthorizedUser cannot exceed 100 characters.")]
        public string? AuthorizedUser { get; set; }
    }

    public enum TransactionType
    {
        Payment,
        Credit
    }
}
