using Application.Features.Auth.CommandHandlers;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Contracts;
using Application.Features.Auth.Models.Responses;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Auth.Commands;

public class LoginCommandHandlerTest
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IJwtProvider _jwtProvider = Substitute.For<IJwtProvider>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultUnauthorized_When_UserNotFound")]
    public async Task Handle_Should_ReturnsResultUnauthorized_When_UserNotFound()
    {
        // Arrange
        var command = new LoginCommand("mail@mail.com", "password");

        _userRepository.GetByEmailAsync(command.Email).ReturnsNull();

        var handler = new LoginCommandHandler(_userRepository, _jwtProvider, _passwordHasher);

        // Act
        Result<LoginResponse> result = await handler.Handle(command, default);

        // Assert
        _jwtProvider.DidNotReceive().GenerateToken(Arg.Any<string>(), Arg.Any<string>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.InvalidCredentials());
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultUnauthorized_When_PasswordIsIncorrect")]
    public async Task Handle_Should_ReturnsResultUnauthorized_When_PasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand("mail@mail.com", "password");

        var user = new User {Email = "mail@mail.com", Password = "hashedPassword"};

        _userRepository.GetByEmailAsync(command.Email).Returns(user);

        _passwordHasher
            .VerifyHashedPassword(user.Password, command.Password)
            .Returns(false);

        var handler = new LoginCommandHandler(_userRepository, _jwtProvider, _passwordHasher);

        // Act
        Result<LoginResponse> result = await handler.Handle(command, default);

        // Assert
        _jwtProvider.DidNotReceive().GenerateToken(Arg.Any<string>(), Arg.Any<string>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.InvalidCredentials());
    }
}
