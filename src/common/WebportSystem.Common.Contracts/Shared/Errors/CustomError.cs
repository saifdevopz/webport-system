namespace WebportSystem.Common.Contracts.Shared.Errors;

public record CustomError
{
    public CustomError(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public static readonly CustomError None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly CustomError NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public static CustomError Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static CustomError NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static CustomError Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static CustomError Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
}