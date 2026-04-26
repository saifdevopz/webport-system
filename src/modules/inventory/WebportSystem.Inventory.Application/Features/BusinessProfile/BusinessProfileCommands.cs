using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Common.Contracts.Shared.Errors;
using WebportSystem.Common.Contracts.Shared.Results;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Application.Features.BusinessProfile;

public sealed record CreateBusinessProfileCommand(
    string BusinessName,
    string Email,
    string Phone,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode,
    string Country,
    string? BankName,
    string? AccountNumber,
    string? BranchCode
) : ICommand<int>;

public class CreateBusinessProfileCommandValidator : AbstractValidator<CreateBusinessProfileCommand>
{
    public CreateBusinessProfileCommandValidator()
    {
        RuleFor(_ => _.BusinessName).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Phone).NotEmpty().MaximumLength(20);
        RuleFor(_ => _.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(_ => _.City).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.Province).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.PostalCode).NotEmpty().MaximumLength(4);
        RuleFor(_ => _.Country).NotEmpty().MaximumLength(100);

        When(_ => _.BankName is not null, () =>
        {
            RuleFor(_ => _.AccountNumber).NotEmpty();
            RuleFor(_ => _.BranchCode).NotEmpty();
        });
    }
}

public sealed class CreateBusinessProfileCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateBusinessProfileCommand, int>
{
    public async Task<Result<int>> Handle(
        CreateBusinessProfileCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.BusinessProfiles
            .AnyAsync(_ => _.Email == command.Email, cancellationToken);

        if (record)
        {
            return Result.Failure<int>(
                CustomError.Problem(nameof(CreateBusinessProfileCommandHandler),
                "Email already exists."));
        }

        var businessProfile = new BusinessProfileM(
            command.BusinessName,
            command.Email,
            command.Phone,
            command.AddressLine1,
            command.City,
            command.Province,
            command.PostalCode,
            command.Country,
            command.BankName,
            command.AccountNumber,
            command.BranchCode);

        await dbContext.BusinessProfiles.AddAsync(businessProfile, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(businessProfile.BusinessProfileId);
    }
}

public sealed record UpdateBusinessProfileCommand(
    int BusinessProfileId,
    string BusinessName,
    string Email,
    string Phone,
    string AddressLine1,
    string City,
    string Province,
    string PostalCode,
    string Country,
    string? BankName,
    string? AccountNumber,
    string? BranchCode,
    string? logoUrl
) : ICommand;

public class UpdateBusinessProfileCommandValidator : AbstractValidator<UpdateBusinessProfileCommand>
{
    public UpdateBusinessProfileCommandValidator()
    {
        RuleFor(_ => _.BusinessProfileId).GreaterThan(0);
        RuleFor(_ => _.BusinessName).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty();
        RuleFor(_ => _.PostalCode).MaximumLength(4);
    }
}

public class UpdateBusinessProfileCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateBusinessProfileCommand>
{
    public async Task<Result> Handle(
        UpdateBusinessProfileCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.BusinessProfiles
            .FindAsync([command.BusinessProfileId], cancellationToken);

        if (record is null)
        {
            return Result.Failure(
                CustomError.NotFound(nameof(UpdateBusinessProfileCommandHandler),
                "Record not found."));
        }

        record.Update(
            command.BusinessName,
            command.Email,
            command.Phone,
            command.AddressLine1,
            command.City,
            command.Province,
            command.PostalCode,
            command.Country,
            command.BankName,
            command.AccountNumber,
            command.BranchCode,
            command.logoUrl
        );

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

