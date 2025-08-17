namespace Utilities.AppDateTime;

public interface IAppDateTime
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}
