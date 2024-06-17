using Application.Features.Accounts.Models.Responses;
using MongoDB.Bson;

namespace Application.Features.Accounts.Queries;

public record GetAccountTransactionsQuery(ObjectId Id) : IQuery<Result<List<AccountTransactionResponse>>>;
