namespace WebportSystem.Common.Contracts.Shared.Results;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure { get; set; }
    public string? Error { get; set; }
    public T? Data { get; set; } = default!;
}