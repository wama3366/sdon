using DDD.Concepts.Interfaces;

namespace SchoolDonations.CoreDomain.Aggregates.Customers.Events;

public class CustomerEventData : IDomainEventData
{
    #region Construction

    public CustomerEventData(Customer customer) => _customer = customer;

    #endregion Construction

    #region Properties

    private readonly Customer _customer;

    #endregion Properties

    public string GetKeyValueString()
    {
        return $"Id:{_customer.Id.Value}";
    }
}
