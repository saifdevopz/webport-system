using Microsoft.AspNetCore.Http;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Common.Presentation.Responses;

public static class ResultExtensions
{
    public static async Task<IResult> MapResult<T>(
        this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.Match(
            _ => Results.Ok(result),
            _ => ApiResults.Problem(result)
        );
    }

    public static async Task<IResult> MapResult(
        this Task<Result> resultTask)
    {
        var result = await resultTask;
        return result.Match(
            () => Results.Ok(result),
            _ => ApiResults.Problem(result)
        );
    }
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Data) : onFailure(result);
    }
}