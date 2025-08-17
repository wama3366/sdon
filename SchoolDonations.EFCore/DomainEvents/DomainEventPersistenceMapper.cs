using DDD.Concepts.BaseTypes;
using Persistence.Concepts;

namespace SchoolDonations.EFCore.DomainEvents;

public class DomainEventPersistenceMapper : IPersistenceMapper<DomainEvent, DomainEventDto>
{
    public DomainEventDto FromDomain(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

        var domainEventDto = new DomainEventDto();
        Copy(domainEvent, domainEventDto);

        return domainEventDto;
    }

    public List<DomainEventDto> FromDomain(IEnumerable<DomainEvent> domainEvents) =>
        domainEvents?.Select(FromDomain).ToList() ?? [];

    public void CopyFromDomain(DomainEvent domainEvent, DomainEventDto domainEventDto)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
        ArgumentNullException.ThrowIfNull(domainEventDto, nameof(domainEventDto));

        Copy(domainEvent, domainEventDto);
    }


    private static void Copy(DomainEvent domainEvent, DomainEventDto domainEventDto)
    {
        domainEventDto.EventType = domainEvent.EventType;
        domainEventDto.EventData = domainEvent.EventData.GetKeyValueString();
        domainEventDto.OccurredOn = domainEvent.OccurredOn;
    }

    #region Not Implemented

    public DomainEvent ToDomain(DomainEventDto domainEventDto)
    {
        throw new NotImplementedException();
    }

    public List<DomainEvent> ToDomain(IEnumerable<DomainEventDto> domainEventDtos)
    {
        throw new NotImplementedException();
    }

    #endregion Not Implemented
}