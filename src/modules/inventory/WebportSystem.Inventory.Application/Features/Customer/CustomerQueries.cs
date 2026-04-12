using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.Customer;

#region Get By Id

public sealed record GetCustomerByIdQuery(int Id) : IQuery<GetCustomerByIdQueryResult>;

public sealed record GetCustomerByIdQueryResult(CustomerDto Record);

public sealed class GetCustomerByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult>
{
    public async Task<Result<GetCustomerByIdQueryResult>> Handle(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Customers
            .AsNoTracking()
            .Select(_ => new CustomerDto
            {
                CustomerId = _.CustomerId,
                Name = _.Name,
                Email = _.Email,
                Phone = _.Phone,
                CompanyName = _.CompanyName,
                AddressLine1 = _.AddressLine1,
                City = _.City,
                Province = _.Province,
                PostalCode = _.PostalCode
            })
            .Where(x => x.CustomerId == query.Id)
            .SingleOrDefaultAsync(cancellationToken);


        return entity is not null
            ? Result.Success(new GetCustomerByIdQueryResult(entity))
            : Result.Failure<GetCustomerByIdQueryResult>(
                CustomError.NotFound("Not Found", "Customer not found."));
    }
}

#endregion

#region Get list

public sealed record GetCustomersQuery : IQuery<GetCustomersQueryResult>;

public sealed record GetCustomersQueryResult(IEnumerable<CustomerDto> Records);

public sealed class GetCustomersQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCustomersQuery, GetCustomersQueryResult>
{
    public async Task<Result<GetCustomersQueryResult>> Handle(
        GetCustomersQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Customers
            .AsNoTracking()
            .Select(_ => new CustomerDto
            {
                CustomerId = _.CustomerId,
                Name = _.Name,
                Email = _.Email,
                Phone = _.Phone,
                CompanyName = _.CompanyName,
                AddressLine1 = _.AddressLine1,
                City = _.City,
                Province = _.Province,
                PostalCode = _.PostalCode
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new GetCustomersQueryResult(records));
    }
}

#endregion