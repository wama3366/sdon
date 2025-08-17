using DDD.Concepts.BaseTypes;

namespace SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;

public interface IDomainEventRepository
{
    Task AddAsync(List<DomainEvent> domainEvents);
}
