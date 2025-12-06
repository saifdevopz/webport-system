using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Item;

public class GetItemsQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetItemsQuery, GetItemsQueryResult>
{
    public async Task<Result<GetItemsQueryResult>> Handle(
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
                Category = new CategoryDto
                {
                    CategoryCode = _.Category!.CategoryCode,
                    CategoryDesc = _.Category.CategoryDesc
                }
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new GetItemsQueryResult(records));
    }
}

public sealed record GetItemsQuery : IQuery<GetItemsQueryResult>;

public sealed record GetItemsQueryResult(IEnumerable<ItemDto> Records);