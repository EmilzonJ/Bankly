using Domain.Enums;
using MongoDB.Bson;

namespace Application.Features.Transactions.Models.Requests;

public class TransactionCreate
{
    public ObjectId SourceAccountId { get; set; }
    public ObjectId? DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public required string Description { get; set; }
    public TransactionType Type { get; set; }
}
