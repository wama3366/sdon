using DDD.Concepts.Interfaces;

namespace DDD.Concepts.BaseTypes;

public abstract record DomainEvent
{
    // Data
    public IDomainEventData EventData {get; init; }
    public Guid UniqueEventId { get; private set; }
    public DateTimeOffset OccurredOn { get; init; }
    public string EventType { get; private set; }
    #region Construction

    public DomainEvent()
    {
        UniqueEventId = Guid.NewGuid();
        EventType = GetType().FullName;
    }

    #endregion Construction
}
