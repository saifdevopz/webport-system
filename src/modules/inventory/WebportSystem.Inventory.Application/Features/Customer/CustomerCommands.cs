using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Application.Features.Customer;

public sealed record CreateCustomerCommand(
    string CustomerName,
    string Email,
    string Phone,
    string CompanyName,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode
) : ICommand<int>;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(_ => _.CustomerName).MaximumLength(50);
        RuleFor(_ => _.Email).EmailAddress();
        RuleFor(_ => _.Phone).MaximumLength(15);
        RuleFor(_ => _.CompanyName).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.AddressLine1).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.PostalCode).NotEmpty().MaximumLength(10);
        RuleFor(_ => _.City).NotEmpty().MaximumLength(50);
        RuleFor(_ => _.Province).NotEmpty().MaximumLength(50);
    }
}

public sealed class CreateCustomerCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateCustomerCommand, int>
{
    public async Task<Result<int>> Handle(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Customers
            .AnyAsync(_ => _.CompanyName == command.CompanyName, cancellationToken);

        if (record)
        {
            return Result.Failure<int>(
                CustomError.Problem(nameof(CreateCustomerCommandHandler),
                "Record already exists."));
        }

        var customer = new CustomerM(
            command.CustomerName,
            command.Email,
            command.Phone,
            command.CompanyName,
            command.AddressLine1,
            command.PostalCode,
            command.City,
            command.Province);

        await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(customer.CustomerId);
    }
}

public sealed record UpdateCustomerCommand(
    int CustomerId,
    string Name,
    string Email,
    string Phone,
    string CompanyName,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode)
: ICommand;

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(_ => _.CustomerId).NotEmpty();
        RuleFor(_ => _.CompanyName).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Name).MaximumLength(100);
        RuleFor(_ => _.Email).EmailAddress();
        RuleFor(_ => _.Phone).MaximumLength(20);
        RuleFor(_ => _.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(_ => _.City).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Province).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.PostalCode).NotEmpty().MaximumLength(10);
    }
}

public sealed class UpdateCustomerCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(
        UpdateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.Customers
            .FindAsync([command.CustomerId], cancellationToken);

        if (record is null)
        {
            return Result.Failure<UpdateCustomerCommand>(
                CustomError.NotFound(nameof(UpdateCustomerCommand),
                "Record not found."));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
