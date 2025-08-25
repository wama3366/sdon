using DDD.Concepts.BaseTypes;
using SchoolDonations.CoreDomain.Aggregates.Customers.Events;
using SchoolDonations.CoreDomain.Values;

namespace SchoolDonations.CoreDomain.Aggregates.Customers;

public readonly record struct CustomerId(long Value) : IIdentityKey;

public class Customer : Entity<CustomerId>
{
    // Data
    public required PersonName Name { get; set; }

    public Address BillingAddress { get; set; }

    public Address ShippingAddress { get; set; }

    public CustomerStatus Status { get; set; }

    #region Construction

    public Customer(CustomerId customerId) : base(customerId)
    {
    }

    public Customer(Customer customer) : base(customer)
    {
        Name = new PersonName(customer.Name);
        BillingAddress = new Address(customer.BillingAddress);
        ShippingAddress = new Address(customer.ShippingAddress);
        Status = customer.Status;
    }

    #endregion Construction

    #region Methods

    public void Activate( DateTimeOffset occurredOn)
    {
        SetStatus(CustomerStatus.Active, occurredOn);
    }

    public void Deactivate( DateTimeOffset occurredOn)
    {
        SetStatus(CustomerStatus.Inactive, occurredOn);
    }

    #region Private Methods

    private void SetStatus(CustomerStatus newStatus, DateTimeOffset occurredOn)
    {
        var originalStatus = Status;
        Status = newStatus;

        var @event = new CustomerStatusChanged(this, originalStatus, newStatus, occurredOn);
        AddDomainEvent(@event);
    }

    #endregion Private Methods

    #endregion Methods
}