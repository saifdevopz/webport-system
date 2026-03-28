using FluentValidation;
using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Identity.Application.Interfaces;

namespace WebportSystem.Identity.Application.Features.Tokens;

#region Access token
public sealed record AccessTokenCommand(string Email, string Password) : ICommand<AccessTokenResult>;

public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    public AccessTokenCommandValidator()
    {
        RuleFor(_ => _.Email).NotEmpty();
        RuleFor(_ => _.Password).NotEmpty();
    }
}

public sealed record AccessTokenResult(string Token, string RefreshToken);

public class AccessTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<AccessTokenCommand, AccessTokenResult>
{
    public async Task<Result<AccessTokenResult>> Handle(
        AccessTokenCommand command,
        CancellationToken cancellationToken)
    {
        Result<TokenResponse> tokenResult = await tokenService.AccessToken(new AccessTokenDto(command.Email, command.Password));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new AccessTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken!));
    }
}
#endregion 

#region Refresh token
public sealed record RefreshTokenCommand(string Token, string RefreshToken) : ICommand<RefreshTokenResult>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(_ => _.Token).NotEmpty();
        RuleFor(_ => _.RefreshToken).NotEmpty();
    }
}

public sealed record RefreshTokenResult(string Token, string RefreshToken);

public class RefreshTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<Result<RefreshTokenResult>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var tokenResult = await tokenService.RefreshToken(new RefreshTokenDto(command.Token, command.RefreshToken));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new RefreshTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken!));
    }
}
#endregion