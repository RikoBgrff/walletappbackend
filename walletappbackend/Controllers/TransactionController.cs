using Microsoft.AspNetCore.Mvc;
using walletappbackend.DataTransferObjects;
using walletappbackend.Entities;
using walletappbackend.Services;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;
    private readonly CardService _cardService;
    private readonly PaymentHistoryService _paymentHistoryService;
    private readonly DailyPointsService _dailyPointsService;

    public TransactionController(
        TransactionService transactionService,
        CardService cardService,
        PaymentHistoryService paymentHistoryService,
        DailyPointsService dailyPointsService)
    {
        _transactionService = transactionService;
        _cardService = cardService;
        _paymentHistoryService = paymentHistoryService;
        _dailyPointsService = dailyPointsService;
    }

    [HttpPost("mark-payment/{userId}")]
    public async Task<IActionResult> MarkPaymentAsPaid(Guid userId)
    {
        var success = await _paymentHistoryService.MarkPaymentAsPaidAsync(userId);
        if (!success)
        {
            return BadRequest($"The payment for {DateTime.UtcNow.ToString("MMMM")} has already been marked as paid.");
        }

        return Ok($"Payment for {DateTime.UtcNow.ToString("MMMM")} marked as paid.");
    }

    [HttpGet("detail/{transactionId}")]
    public async Task<IActionResult> GetTransactionDetail(Guid transactionId)
    {
        var transaction = await _transactionService.GetTransactionDetailAsync(transactionId);
        if (transaction == null) return NotFound($"Transaction with ID {transactionId} not found.");

        return Ok(transaction);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        // Validation automatically triggers. If invalid, 400 response is returned.
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Type = request.Type,
            Amount = request.Amount,
            Name = request.Name,
            Description = request.Description,
            Date = DateTime.UtcNow,
            IsPending = request.IsPending,
            AuthorizedUser = request.AuthorizedUser
        };

        var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);
        return CreatedAtAction(nameof(GetTransactionDetail), new { transactionId = createdTransaction.Id }, createdTransaction);
    }


    [HttpDelete("delete/{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        var success = await _transactionService.DeleteTransactionAsync(transactionId);
        if (!success) return NotFound($"Transaction with ID {transactionId} not found.");

        return NoContent();
    }

    [HttpGet("list/{userId}")]
    public async Task<IActionResult> GetTransactionsByUser(Guid userId)
    {
        var transactions = await _transactionService.GetTransactionsByUserAsync(userId);
        var card = await _cardService.GetCardByUserAsync(userId);

        if (card == null) return NotFound($"Card for user ID {userId} not found.");

        var paymentHistory = await _paymentHistoryService.GetCurrentPaymentHistoryAsync(userId, DateTime.UtcNow.Year, DateTime.UtcNow.Month);

        var paymentMessage = paymentHistory != null && paymentHistory.IsPaid
            ? $"You’ve paid your {DateTime.UtcNow.ToString("MMMM")} balance."
            : $"Your {DateTime.UtcNow.ToString("MMMM")} balance is due.";

        var response = new
        {
            CardBalance = new { Balance = card.Balance, Available = card.Limit - card.Balance },
            NoPaymentDue = paymentMessage,
            DailyPoints = await _dailyPointsService.CalculateDailyPointsAsync(DateTime.UtcNow, userId),
            Transactions = transactions
        };

        return Ok(response);
    }
}
