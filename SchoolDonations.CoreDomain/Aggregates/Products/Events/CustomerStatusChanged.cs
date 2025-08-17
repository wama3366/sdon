using DDD.Concepts.BaseTypes;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers.Events;

namespace SchoolDonations.CoreDomain.Aggregates.Products.Events;

public record ProductAdded : DomainEvent
{
    #region Properties

    #endregion Properties

    #region Construction

    public ProductAdded(Product product, DateTimeOffset occurredOn)
    {
        EventData = new ProductEventData(product);
        OccurredOn = occurredOn;
    }

    #endregion Construction
}