using FluentValidation;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Domain.Results;
using WebportSystem.Identity.Application.Dtos;
using WebportSystem.Identity.Application.Interfaces;

namespace WebportSystem.Identity.Application.Features.Tokens;

public class AccessTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<AccessTokenCommand, AccessTokenResult>
{
    public async Task<Result<AccessTokenResult>> Handle(
        AccessTokenCommand command,
        CancellationToken cancellationToken)
    {
        Result<TokenResponse> tokenResult = await tokenService.AccessToken(new AccessTokenRequest(command.Email, command.Password));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new AccessTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken!));
    }
}

public sealed record AccessTokenCommand(string Email, string Password) : ICommand<AccessTokenResult>;

public sealed record AccessTokenResult(string Token, string RefreshToken);

public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    public AccessTokenCommandValidator()
    {
        RuleFor(_ => _.Email).NotEmpty();
        RuleFor(_ => _.Password).NotEmpty();
    }
}