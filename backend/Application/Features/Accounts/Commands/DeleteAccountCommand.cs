using MongoDB.Bson;

namespace Application.Features.Accounts.Commands;

public record DeleteAccountCommand(ObjectId Id) : ICommand<Result>;
