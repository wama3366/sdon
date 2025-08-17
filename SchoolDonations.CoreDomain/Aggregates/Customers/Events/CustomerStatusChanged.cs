using DDD.Concepts.BaseTypes;

namespace SchoolDonations.CoreDomain.Aggregates.Customers.Events;

public record CustomerStatusChanged : DomainEvent
{
    #region Properties

    public CustomerStatus OriginStatus { get; init; }
    public CustomerStatus NewStatus { get; init; }

    #endregion Properties

    #region Construction

    public CustomerStatusChanged(Customer customer, CustomerStatus originalStatus, CustomerStatus newStatus, DateTimeOffset occurredOn)
    {
        EventData = new CustomerEventData(customer);
        OriginStatus = originalStatus;
        NewStatus = newStatus;
        OccurredOn = occurredOn;
    }

    #endregion Construction
}