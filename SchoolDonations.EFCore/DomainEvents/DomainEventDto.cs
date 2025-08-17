using Persistence.Concepts;

namespace SchoolDonations.EFCore.DomainEvents;

public class DomainEventDto : Dto
{
    public Guid UniqueEventId { get; set; }
    public string EventType { get; set; }
    public string EventData { get; set; }
    public DateTimeOffset OccurredOn { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? PublishedOn { get; set; }
}
