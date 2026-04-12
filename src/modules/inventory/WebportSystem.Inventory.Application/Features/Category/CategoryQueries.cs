using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.Category;

public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<CategoryDto>;

public class GetCategoryByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Categories
            .FindAsync([query.CategoryId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<CategoryDto>(
                CustomError.NotFound(nameof(GetCategoryByIdQueryHandler),
                "Record not found."));
        }

        return Result.Success(new CategoryDto
        {
            CategoryId = record.CategoryId,
            CategoryCode = record.CategoryCode,
            CategoryDesc = record.CategoryDesc,
        });
    }
}

public sealed record GetCategoriesQuery : IQuery<List<CategoryDto>>;

public class GetCategoriesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<Result<List<CategoryDto>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Categories
            .AsNoTracking()
            .Select(_ => new CategoryDto
            {
                CategoryId = _.CategoryId,
                CategoryCode = _.CategoryCode,
                CategoryDesc = _.CategoryDesc,
            })
            .ToListAsync(cancellationToken);

        return Result.Success(records);
    }
}
