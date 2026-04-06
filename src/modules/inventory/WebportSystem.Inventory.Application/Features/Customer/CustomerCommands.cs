using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.Customer;

namespace WebportSystem.Inventory.Application.Features.Customer;

public sealed record CreateCustomerCommand(CustomerDto Customer) : ICommand;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(_ => _.Customer.Name).MaximumLength(50);
        RuleFor(_ => _.Customer.Email).EmailAddress();
        RuleFor(_ => _.Customer.Phone).MaximumLength(15);
        RuleFor(_ => _.Customer.CompanyName).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Customer.AddressLine1).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Customer.PostalCode).NotEmpty().MaximumLength(10);
        RuleFor(_ => _.Customer.City).NotEmpty().MaximumLength(50);
        RuleFor(_ => _.Customer.Province).NotEmpty().MaximumLength(50);
    }
}

public sealed class CreateCustomerCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateCustomerCommand>
{
    public async Task<Result> Handle(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.Customers
            .AnyAsync(_ => _.CompanyName == command.Customer.CompanyName, cancellationToken);

        if (exists)
        {
            return Result.Failure<CreateCustomerCommand>(
                CustomError.Problem(nameof(CreateCustomerCommand), "Record already exists."));
        }

        var customer = new CustomerM(
            command.Customer.Name!,
            command.Customer.Email!,
            command.Customer.Phone!,
            command.Customer.CompanyName,
            command.Customer.AddressLine1,
            command.Customer.PostalCode,
            command.Customer.City,
            command.Customer.Province);

        await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateCustomerCommand(
    int CustomerId,
    string? Name,
    string? Email,
    string? Phone,
    string CompanyName,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode) : ICommand;

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
                CustomError.NotFound(nameof(UpdateCustomerCommand), "Record not found."));
        }

        dbContext.Customers.Update(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
