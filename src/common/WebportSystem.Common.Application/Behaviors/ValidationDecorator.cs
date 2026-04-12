using FluentValidation;
using FluentValidation.Results;
using WebportSystem.Common.Application.Abstractions;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Common.Application.Behaviors;

internal static class ValidationDecorator
{
    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> inner,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TCommand>(command);
            ValidationResult[] validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            ValidationFailure[] failures = [.. validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)];

            if (failures.Length != 0)
            {
                return Result.Failure(CreateValidationError(failures));
            }

            return await inner.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> inner,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TCommand>(command);
            ValidationResult[] validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            ValidationFailure[] failures = [.. validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)];

            if (failures.Length != 0)
            {
                return Result.Failure<TResponse>(CreateValidationError(failures));
            }

            return await inner.Handle(command, cancellationToken);
        }
    }

    private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
        new([.. validationFailures.Select(f => CustomError.Problem(f.ErrorCode, f.ErrorMessage))]);
}
