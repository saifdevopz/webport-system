namespace WebportSystem.Inventory.Application.Features.Category;

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

public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<GetCategoryByIdQuery>;

public sealed record GetCategoryByIdQueryResult(CategoryM Record);
