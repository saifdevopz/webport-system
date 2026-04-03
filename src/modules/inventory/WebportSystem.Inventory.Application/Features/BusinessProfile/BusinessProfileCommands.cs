using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebportSystem.Inventory.Domain.Entities.BusinessProfile;

namespace WebportSystem.Inventory.Application.Features.BusinessProfile;

#region Create
public sealed record CreateBusinessProfileCommand(BusinessProfileDto BusinessProfile) : ICommand;

public class CreateBusinessProfileCommandValidator : AbstractValidator<CreateBusinessProfileCommand>
{
    public CreateBusinessProfileCommandValidator()
    {
        RuleFor(_ => _.BusinessProfile.BusinessName).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.BusinessProfile.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.BusinessProfile.Phone).NotEmpty().MaximumLength(20);
        RuleFor(_ => _.BusinessProfile.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(_ => _.BusinessProfile.City).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.BusinessProfile.Province).NotEmpty().MaximumLength(100);
        RuleFor(_ => _.BusinessProfile.PostalCode).NotEmpty().MaximumLength(10);
        RuleFor(_ => _.BusinessProfile.Country).NotEmpty().MaximumLength(100);

        When(_ => _.BusinessProfile.BankName is not null, () =>
        {
            RuleFor(_ => _.BusinessProfile.AccountNumber).NotEmpty();
            RuleFor(_ => _.BusinessProfile.BranchCode).NotEmpty();
        });
    }
}

public sealed class CreateBusinessProfileCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<CreateBusinessProfileCommand>
{
    public async Task<Result> Handle(
        CreateBusinessProfileCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.BusinessProfiles
            .AnyAsync(_ => _.BusinessName == command.BusinessProfile.BusinessName, cancellationToken);

        if (exists)
        {
            return Result.Failure(
                CustomError.Problem(nameof(CreateBusinessProfileCommand), "A business profile with this name already exists."));
        }

        var businessProfile = new BusinessProfileM(
            command.BusinessProfile.BusinessName,
            command.BusinessProfile.Email,
            command.BusinessProfile.Phone,
            command.BusinessProfile.AddressLine1,
            command.BusinessProfile.City,
            command.BusinessProfile.Province,
            command.BusinessProfile.PostalCode,
            command.BusinessProfile.Country,
            command.BusinessProfile.BankName,
            command.BusinessProfile.AccountNumber,
            command.BusinessProfile.BranchCode);

        await dbContext.BusinessProfiles.AddAsync(businessProfile, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
#endregion

#region Update
public sealed record UpdateBusinessProfileCommand(int BusinessProfileId, string BusinessProfileDesc) : ICommand<UpdateBusinessProfileResult>;

public sealed record UpdateBusinessProfileResult(BusinessProfileM Result);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateBusinessProfileCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.BusinessProfileId).NotEmpty();
        RuleFor(_ => _.BusinessProfileDesc).NotEmpty();
    }
}
public class UpdateBusinessProfileCommandHandler(IInventoryDbContext dbContext)
    : ICommandHandler<UpdateBusinessProfileCommand, UpdateBusinessProfileResult>
{
    public async Task<Result<UpdateBusinessProfileResult>> Handle(
        UpdateBusinessProfileCommand command,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.BusinessProfiles.FindAsync([command.BusinessProfileId], cancellationToken);

        if (record == null)
        {
            return Result.Failure<UpdateBusinessProfileResult>(
                CustomError.NotFound(nameof(UpdateBusinessProfileCommandHandler), "Record not found."));
        }

        dbContext.BusinessProfiles.Update(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateBusinessProfileResult(record));
    }
}
#endregion

