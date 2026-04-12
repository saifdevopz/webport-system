using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.BusinessProfile;

public sealed record GetBusinessProfileByIdQuery(int BusinessProfileId) : IQuery<BusinessProfileDto>;

public class GetBusinessProfileByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetBusinessProfileByIdQuery, BusinessProfileDto>
{
    public async Task<Result<BusinessProfileDto>> Handle(
        GetBusinessProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.BusinessProfiles
            .FindAsync([query.BusinessProfileId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<BusinessProfileDto>(
                CustomError.NotFound(nameof(GetBusinessProfileByIdQueryHandler),
                "Record not found."));
        }

        return Result.Success(new BusinessProfileDto
        {
            BusinessProfileId = record.BusinessProfileId,
            BusinessName = record.BusinessName,
            Email = record.Email,
            Phone = record.Phone,
            AddressLine1 = record.AddressLine1,
            City = record.City,
            Province = record.Province,
            PostalCode = record.PostalCode,
            Country = record.Country,
            BankName = record.BankName,
            AccountNumber = record.AccountNumber,
            BranchCode = record.BranchCode
        });
    }
}

public sealed record GetBusinessProfilesQuery : IQuery<List<BusinessProfileDto>>;

public class GetBusinessProfilesQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetBusinessProfilesQuery, List<BusinessProfileDto>>
{
    public async Task<Result<List<BusinessProfileDto>>> Handle(
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

        return Result.Success(records);
    }
}
