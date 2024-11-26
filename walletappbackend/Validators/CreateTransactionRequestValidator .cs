using FluentValidation;
using walletappbackend.DataTransferObjects;

namespace walletappbackend.Validators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Type)
                .IsEnumName(typeof(TransactionType), caseSensitive: false)
                .WithMessage("Type must be 'Payment' or 'Credit'.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.AuthorizedUser)
                .MaximumLength(100).WithMessage("AuthorizedUser cannot exceed 100 characters.");
        }
    }
}
