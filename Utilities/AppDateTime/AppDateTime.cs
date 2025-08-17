namespace Utilities.AppDateTime;

public class AppDateTime : IAppDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}
