using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Common.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
