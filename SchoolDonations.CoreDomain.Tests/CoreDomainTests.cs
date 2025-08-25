using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;

namespace SchoolDonations.CoreDomain.Tests;

public class CoreDomainTests
{
    [Fact]
    public void CustomerCopyConstructorCopiesStatus()
    {
        var address = new Address
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 4",
            City = "Townsville",
            State = UsState.GetByAbbreviation("CA"),
            ZipCode = new ZipCode("12345"),
            Country = "USA"
        };

        var original = new Customer(new CustomerId(1))
        {
            Name = new PersonName { FirstName = "John", LastName = "Doe" },
            BillingAddress = address,
            ShippingAddress = address,
            Status = CustomerStatus.Active
        };

        var copy = new Customer(original);

        Assert.Equal(CustomerStatus.Active, copy.Status);
    }

    [Fact]
    public void AddressToStringContainsAddressLine2()
    {
        var address = new Address
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 4",
            City = "Townsville",
            State = UsState.GetByAbbreviation("CA"),
            ZipCode = new ZipCode("12345"),
            Country = "USA"
        };

        var result = address.ToString();

        Assert.Contains(address.AddressLine2, result);
    }
}
