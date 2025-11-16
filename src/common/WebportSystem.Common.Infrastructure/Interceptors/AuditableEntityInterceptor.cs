using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebportSystem.Common.Domain.Abstractions;
using WebportSystem.Common.Infrastructure.Database;

namespace WebportSystem.Common.Infrastructure.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        UpdateAuditEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        UpdateAuditEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public static void UpdateAuditEntities(DbContext? context)
    {
        if (context != null)
        {
            var tenantProvider = context.GetService<TenantProvider>();

            foreach (EntityEntry<IAuditable> entry in context.ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = tenantProvider.UserEmail;
                    entry.Entity.CreatedDt = TimeProvider.Now;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModBy = tenantProvider.UserEmail;
                    entry.Entity.LastModDt = TimeProvider.Now;
                }
            }
        }
    }
}

public static class AuditableExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        return entry.References.Any(r =>
                    r.TargetEntry != null &&
                    r.TargetEntry.Metadata.IsOwned() &&
                    (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}

public static class TimeProvider
{
    public static DateTime Now => DateTime.UtcNow.AddHours(2);
}