using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Inventory;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;

namespace WebportSystem.Inventory.Application.Features.Customer;

public sealed record GetCustomerByIdQuery(int CustomerId) : IQuery<CustomerDto>;

public sealed class GetCustomerByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<Result<CustomerDto>> Handle(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Customers
            .AsNoTracking()
            .Select(_ => new CustomerDto
            {
                CustomerId = _.CustomerId,
                CustomerName = _.Name,
                Email = _.Email,
                Phone = _.Phone,
                CompanyName = _.CompanyName,
                AddressLine1 = _.AddressLine1,
                City = _.City,
                Province = _.Province,
                PostalCode = _.PostalCode
            })
            .Where(x => x.CustomerId == query.CustomerId)
            .SingleOrDefaultAsync(cancellationToken);


        return entity is not null
            ? Result.Success(entity)
            : Result.Failure<CustomerDto>(
                CustomError.NotFound("Not Found", "Customer not found."));
    }
}

public sealed record GetCustomersQuery : IQuery<IEnumerable<CustomerDto>>;

public sealed class GetCustomersQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCustomersQuery, List<CustomerDto>>
{
    public async Task<Result<List<CustomerDto>>> Handle(
        GetCustomersQuery query,
        CancellationToken cancellationToken)
    {
        var records = await dbContext.Customers
            .AsNoTracking()
            .Select(_ => new CustomerDto
            {
                CustomerId = _.CustomerId,
                CustomerName = _.Name,
                Email = _.Email,
                Phone = _.Phone,
                CompanyName = _.CompanyName,
                AddressLine1 = _.AddressLine1,
                City = _.City,
                Province = _.Province,
                PostalCode = _.PostalCode
            })
            .ToListAsync(cancellationToken);

        return Result.Success(records);
    }
}