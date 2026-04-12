using WebportSystem.Common.Contracts.Shared.Errors;

namespace WebportSystem.Common.Contracts.Shared.Exceptions;

public sealed class CustomException(string requestName,
                                    CustomError? error = default,
                                    Exception? innerException = default)
    : Exception("Application exception", innerException)
{
    public string RequestName { get; } = requestName;

    public CustomError? Error { get; } = error;
}
