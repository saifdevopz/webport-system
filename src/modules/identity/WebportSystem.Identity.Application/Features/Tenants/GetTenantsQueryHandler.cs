using Microsoft.EntityFrameworkCore;
using WebportSystem.Identity.Application.Data;
using WebportSystem.Identity.Domain.Tenants;

namespace WebportSystem.Identity.Application.Features.Tenants;

internal sealed class GetTenantsQueryHandler(IUsersDbContext dbContext)
    : IQueryHandler<GetTenantsQuery, GetTenantsQueryResult>
{
    public async Task<Result<GetTenantsQueryResult>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellationToken)
    {
        var model = await dbContext.Tenants.ToListAsync(cancellationToken);

        return Result.Success(new GetTenantsQueryResult(model));
    }
}

public sealed record GetTenantsQuery : IQuery<GetTenantsQueryResult>;

public sealed record GetTenantsQueryResult(IEnumerable<TenantM> Tenants);
