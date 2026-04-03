using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Application.Features.Customer;

#region Get By Id

public sealed record GetCustomerByIdQuery(int Id) : IQuery<GetCustomerByIdQueryResult>;

public sealed record GetCustomerByIdQueryResult(CustomerM Customer);

public sealed class GetCustomerByIdQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult>
{
    public async Task<Result<GetCustomerByIdQueryResult>> Handle(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Customers
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
            .Select(_ => new CustomerDto(
                _.CustomerId,
                _.Name,
                _.Email,
                _.Phone,
                _.CompanyName,
                _.AddressLine1,
                _.City,
                _.Province,
                _.PostalCode))
            .ToListAsync(cancellationToken);

        return Result.Success(new GetCustomersQueryResult(records));
    }
}

#endregion