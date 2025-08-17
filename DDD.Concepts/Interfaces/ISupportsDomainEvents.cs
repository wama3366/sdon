using DDD.Concepts.BaseTypes;

namespace DDD.Concepts.Interfaces;

public interface ISupportsDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
