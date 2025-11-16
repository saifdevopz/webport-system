namespace WebportSystem.Common.Infrastructure.Clock;

public interface IDateTimeProvider
{
    public DateTime Now { get; }
}

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow.AddHours(2);
}