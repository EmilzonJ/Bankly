namespace Application.Features.Auth.Models.Responses;

public record LoginResponse(
    string Token,
    string Email
);
