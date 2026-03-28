using Microsoft.EntityFrameworkCore;

namespace WebportSystem.Inventory.Application.Features.Item;

public sealed record GetItemByIdQuery(int ItemId) : IQuery<GetItemByIdQuery>;

public sealed record GetItemByIdQueryResult(ItemM Record);

public class GetItemByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResult>
{
    public async Task<Result<GetItemByIdQueryResult>> Handle(
        GetItemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Items.FindAsync([query.ItemId], cancellationToken);

        return record is null
            ? Result.Failure<GetItemByIdQueryResult>(CustomError.NotFound(nameof(GetItemByIdQueryHandler), "Record not found."))
            : Result.Success(new GetItemByIdQueryResult(record));
    }
}


public sealed record GetItemsQuery : IQuery<GetItemsQueryResult>;

public sealed record GetItemsQueryResult(IEnumerable<ItemDto> Records);

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