using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Application.Features.Tenants;

public sealed record GetTenantByIdQuery(int TenantId) : IQuery<GetTenantByIdQueryResult>;

public sealed record GetTenantByIdQueryResult(TenantM Tenant);

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

public sealed record GetTenantsQuery : IQuery<GetTenantsQueryResult>;

public sealed record GetTenantsQueryResult(IEnumerable<TenantDto> Tenants);

internal sealed class GetTenantsQueryHandler(IUsersDbContext dbContext)
    : IQueryHandler<GetTenantsQuery, GetTenantsQueryResult>
{
    public async Task<Result<GetTenantsQueryResult>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Tenants
            .AsNoTracking()
            .Select(_ => new TenantDto
            {
                TenantId = _.TenantId,
                TenantName = _.TenantName,
                DatabaseName = _.DatabaseName
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new GetTenantsQueryResult(model));
    }
}
