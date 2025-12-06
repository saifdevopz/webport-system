using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Category;

public class GetCategoriesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResult>
{
    public async Task<Result<GetCategoriesQueryResult>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(new GetCategoriesQueryResult(records));
    }
}

public sealed record GetCategoriesQuery : IQuery<GetCategoriesQueryResult>;

public sealed record GetCategoriesQueryResult(IEnumerable<CategoryM> Records);