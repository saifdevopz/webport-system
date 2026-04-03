using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Application.Features.BusinessProfile;

public sealed record GetBusinessProfileByIdQuery(int BusinessProfileId) : IQuery<GetBusinessProfileByIdQuery>;

public sealed record GetBusinessProfileByIdQueryResult(BusinessProfileM Record);

public class GetBusinessProfileByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetBusinessProfileByIdQuery, GetBusinessProfileByIdQueryResult>
{
    public async Task<Result<GetBusinessProfileByIdQueryResult>> Handle(
        GetBusinessProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.BusinessProfiles.FindAsync([query.BusinessProfileId], cancellationToken);

        return record is null
            ? Result.Failure<GetBusinessProfileByIdQueryResult>(CustomError.NotFound(nameof(GetBusinessProfileByIdQueryHandler), "Record not found."))
            : Result.Success(new GetBusinessProfileByIdQueryResult(record));
    }
}

public sealed record GetBusinessProfilesQuery : IQuery<GetBusinessProfilesQueryResult>;

public sealed record GetBusinessProfilesQueryResult(IEnumerable<BusinessProfileDto> Records);

public class GetBusinessProfilesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetBusinessProfilesQuery, GetBusinessProfilesQueryResult>
{
    public async Task<Result<GetBusinessProfilesQueryResult>> Handle(
        GetBusinessProfilesQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.BusinessProfiles
            .AsNoTracking()
            .Select(_ => new BusinessProfileDto
            {
                BusinessProfileId = _.BusinessProfileId,
                BusinessName = _.BusinessName,
                Email = _.Email,
                Phone = _.Phone,
                AddressLine1 = _.AddressLine1,
                City = _.City,
                Province = _.Province,
                PostalCode = _.PostalCode,
                Country = _.Country,
                BankName = _.BankName,
                AccountNumber = _.AccountNumber,
                BranchCode = _.BranchCode
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new GetBusinessProfilesQueryResult(records));
    }
}
