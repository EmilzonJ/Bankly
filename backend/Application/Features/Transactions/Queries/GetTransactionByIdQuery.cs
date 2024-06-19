using Application.Features.Transactions.Models.Responses;
using MongoDB.Bson;

namespace Application.Features.Transactions.Queries;

public record GetTransactionByIdQuery(ObjectId Id) : IQuery<Result<TransactionDetailResponse>>;
