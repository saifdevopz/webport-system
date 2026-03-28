using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Category;

public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<GetCategoryByIdQuery>;

public sealed record GetCategoryByIdQueryResult(CategoryM Record);

public class GetCategoryByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
{
    public async Task<Result<GetCategoryByIdQueryResult>> Handle(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Categories.FindAsync([query.CategoryId], cancellationToken);

        return record is null
            ? Result.Failure<GetCategoryByIdQueryResult>(CustomError.NotFound(nameof(GetCategoryByIdQueryHandler), "Record not found."))
            : Result.Success(new GetCategoryByIdQueryResult(record));
    }
}

public sealed record GetCategoriesQuery : IQuery<GetCategoriesQueryResult>;

public sealed record GetCategoriesQueryResult(IEnumerable<CategoryM> Records);

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
