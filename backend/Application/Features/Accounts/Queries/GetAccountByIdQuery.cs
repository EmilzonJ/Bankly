using Application.Features.Accounts.Models.Responses;
using MongoDB.Bson;

namespace Application.Features.Accounts.Queries;

public record GetAccountByIdQuery(ObjectId Id) : IQuery<Result<AccountResponse>>;
