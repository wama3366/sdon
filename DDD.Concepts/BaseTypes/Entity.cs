using DDD.Concepts.Interfaces;

namespace DDD.Concepts.BaseTypes;

public interface IIdentityKey
{
    long Value { get; }
}
public abstract class Entity<TId> : IEquatable<Entity<TId>>, ISupportsDomainEvents
    where TId : struct, IEquatable<TId>, IIdentityKey
{
    public TId Id { get; }

    #region Construction

    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity(Entity<TId> entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        Id = entity.Id;
    }

    #endregion Construction

    #region Override Equality

    public override bool Equals(object obj)
        => obj is Entity<TId> other && Equals(other);

    public bool Equals(Entity<TId> other)
        => other != null && GetType() == other.GetType() && Id.Equals(other.Id);

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
        => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
        => !Equals(left, right);

    #endregion Override Equality

    #region Event Handling

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    #endregion Event Handling
}




