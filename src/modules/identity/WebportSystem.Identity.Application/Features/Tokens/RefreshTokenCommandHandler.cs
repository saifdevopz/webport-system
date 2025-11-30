using FluentValidation;
using WebportSystem.Common.Domain.Contracts.Identity;
using WebportSystem.Identity.Application.Interfaces;

namespace WebportSystem.Identity.Application.Features.Tokens;

public class RefreshTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<Result<RefreshTokenResult>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var tokenResult = await tokenService.RefreshToken(new RefreshTokenRequest(command.Token, command.RefreshToken));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<RefreshTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new RefreshTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken!));
    }
}

public sealed record RefreshTokenCommand(string Token, string RefreshToken) : ICommand<RefreshTokenResult>;

public sealed record RefreshTokenResult(string Token, string RefreshToken);

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(_ => _.Token).NotEmpty();
        RuleFor(_ => _.RefreshToken).NotEmpty();
    }
}