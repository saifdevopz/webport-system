using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Application.Features.Tenants;

public class GetTenantByIdQueryHandler(IUsersDbContext dbContext)
    : IQueryHandler<GetTenantByIdQuery, GetTenantByIdQueryResult>
{
    public async Task<Result<GetTenantByIdQueryResult>> Handle(
        GetTenantByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Tenants.FindAsync([query.TenantId], cancellationToken);

        return model is not null
            ? Result.Success(new GetTenantByIdQueryResult(model))
            : Result.Failure<GetTenantByIdQueryResult>(CustomError.NotFound("Not Found", "Tenant not found."));
    }
}

public sealed record GetTenantByIdQuery(int TenantId) : IQuery<GetTenantByIdQueryResult>;

public sealed record GetTenantByIdQueryResult(TenantM Tenant);