
using WebportSystem.Common.Domain.Errors;

namespace WebportSystem.Common.Infrastructure.Exceptions;

public sealed class CustomException(string requestName,
                                    CustomError? error = default,
                                    Exception? innerException = default)
    : Exception("Application exception", innerException)
{
    public string RequestName { get; } = requestName;

    public CustomError? Error { get; } = error;
}
