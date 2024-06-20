using Application.Features.Auth.Commands;
using Application.Features.Auth.Contracts;
using Application.Features.Auth.Models.Responses;

namespace Application.Features.Auth.CommandHandlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher
) : ICommandHandler<LoginCommand, Result<LoginResponse>>
{
    public async ValueTask<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);

        if (user is null)
            return Result.Failure<LoginResponse>(AuthErrors.InvalidCredentials());

        if (!passwordHasher.VerifyHashedPassword(command.Password, user.Password))
            return Result.Failure<LoginResponse>(AuthErrors.InvalidCredentials());

        var token = jwtProvider.GenerateToken(user.Id.ToString(), user.Email);

        return new LoginResponse(token, user.Email);
    }
}
