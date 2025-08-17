using Microsoft.EntityFrameworkCore;
using DDD.Concepts.BaseTypes;
using Persistence.Concepts;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;
using Utilities.AppDateTime;

namespace SchoolDonations.EFCore.DomainEvents;

public class DomainEventRepository : IDomainEventRepository
{
    private DbSet<DomainEventDto> DomainEvents { get; }
    private IPersistenceMapper<DomainEvent, DomainEventDto> DomainEventMapper { get; }
    private IAppDateTime AppDateTime { get; }

    #region Construction

    public DomainEventRepository(
        AppDbContext appDbContext,
        IAppDateTime appDateTime,
        IPersistenceMapper<DomainEvent, DomainEventDto> domainEventMapper)
    {
        DomainEvents = appDbContext?.DomainEvents ?? throw new ArgumentNullException(nameof(appDbContext));
        AppDateTime = appDateTime ?? throw new ArgumentNullException(nameof(appDateTime));
        DomainEventMapper = domainEventMapper ?? throw new ArgumentNullException(nameof(domainEventMapper));
    }

    #endregion Construction

    #region Queries

    #endregion Queries

    #region Commands

    public async Task AddAsync(List<DomainEvent> domainEvents)
    {
        var domainEventDtos = DomainEventMapper.FromDomain(domainEvents);
        domainEventDtos.ForEach(x => x.PublishedOn = AppDateTime.UtcNow);

        await DomainEvents.AddRangeAsync(domainEventDtos);
    }

    #endregion Commands
}
