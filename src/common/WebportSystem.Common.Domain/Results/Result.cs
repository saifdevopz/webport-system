using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WebportSystem.Common.Domain.Errors;

namespace WebportSystem.Common.Domain.Results;

public class Result(bool isSuccess, CustomError error, string? message = null)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public CustomError? Error { get; } = isSuccess ? null : error;

    public string? Message { get; } = message;

    public static Result Success() => new(true, CustomError.None);
    public static Result<T> Success<T>(T data) => data is not null
        ? new Result<T>(data, true, CustomError.None)
        : throw new ArgumentNullException(nameof(data), "Value cannot be null for success");

    public static Result Failure(CustomError error) => new(false, error);
    public static Result<T> Failure<T>(CustomError error) => new(default, false, error);

}

public class Result<T>(T? data, bool isSuccess, CustomError error) : Result(isSuccess, error)
{
    private readonly T? _data = data;

    [JsonPropertyOrder(99)]
    [NotNull]
    public T Data => IsSuccess
        ? _data!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");
}

public record ListWrapper<T>(IEnumerable<T> Records);
public record Wrapper<T>(T Record);
