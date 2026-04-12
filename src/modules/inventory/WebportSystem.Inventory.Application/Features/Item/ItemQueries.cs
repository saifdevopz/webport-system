using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Application.Features.BusinessProfile;

namespace WebportSystem.Inventory.Application.Features.Item;

public sealed record GetItemByIdQuery(int ItemId) : IQuery<ItemDto>;

public class GetItemByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetItemByIdQuery, ItemDto>
{
    public async Task<Result<ItemDto>> Handle(
        GetItemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items
            .AsNoTracking()
            .Select(_ => new ItemDto
            {
                ItemId = _.ItemId,
                ItemCode = _.ItemCode,
                ItemDesc = _.ItemDesc,
                SellingPrice = _.SellingPrice,
                CostPrice = _.CostPrice,
                CategoryId = _.CategoryId,
                Category = new CategoryDto
                {
                    CategoryId = _.CategoryId,
                    CategoryCode = _.Category!.CategoryCode,
                    CategoryDesc = _.Category.CategoryDesc
                }
            })
            .FirstOrDefaultAsync(_ => _.ItemId == query.ItemId, cancellationToken);

        if (record == null)
        {
            return Result.Failure<ItemDto>(
                CustomError.NotFound(nameof(GetBusinessProfileByIdQueryHandler),
                "Record not found."));
        }

        return Result.Success(record);
    }
}

public sealed record GetItemsQuery : IQuery<List<ItemDto>>;

public class GetItemsQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetItemsQuery, List<ItemDto>>
{
    public async Task<Result<List<ItemDto>>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken)
    {
        List<ItemDto> records = await dbContext.Items
            .AsNoTracking()
            .Include(i => i.Category)
            .Select(_ => new ItemDto
            {
                ItemId = _.ItemId,
                ItemCode = _.ItemCode,
                ItemDesc = _.ItemDesc,
                SellingPrice = _.SellingPrice,
                CostPrice = _.CostPrice,
                Category = new CategoryDto
                {
                    CategoryCode = _.Category!.CategoryCode,
                    CategoryDesc = _.Category.CategoryDesc
                }
            })
            .ToListAsync(cancellationToken);

        return Result.Success(records);
    }
}