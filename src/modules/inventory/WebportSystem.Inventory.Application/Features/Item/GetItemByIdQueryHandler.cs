namespace WebportSystem.Inventory.Application.Features.Item;

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

public sealed record GetItemByIdQuery(int ItemId) : IQuery<GetItemByIdQuery>;

public sealed record GetItemByIdQueryResult(ItemM Record);
