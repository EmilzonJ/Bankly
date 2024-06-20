using Application.Features.Auth.Models.Responses;

namespace Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : ICommand<Result<LoginResponse>>;
