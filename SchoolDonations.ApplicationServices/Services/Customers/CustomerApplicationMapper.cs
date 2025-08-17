using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerApplicationMapper : IApplicationMapper<Customer, CustomerApplicationDto>
{
    public Customer ToDomain(CustomerApplicationDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customerDto, nameof(customerDto));

        var customer = new Customer(new CustomerId(customerDto.Id))
        {
            Name = new PersonName()
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
            },
            BillingAddress = new Address()
            {
                AddressLine1 = customerDto.BillingAddressLine1,
                AddressLine2 = customerDto.BillingAddressLine2,
                City = customerDto.BillingCity,
                State = UsState.GetByAbbreviation(customerDto.BillingState),
                ZipCode = new ZipCode(customerDto.BillingZipCode),
                Country = customerDto.BillingCountry,
            },
            ShippingAddress = new Address()
            {
                AddressLine1 = customerDto.ShippingAddressLine1,
                AddressLine2 = customerDto.ShippingAddressLine2,
                City = customerDto.ShippingCity,
                State = UsState.GetByAbbreviation(customerDto.ShippingState),
                ZipCode = new ZipCode(customerDto.ShippingZipCode),
                Country = customerDto.ShippingCountry,
            }
        };

        return customer;
    }

    public void CopyFromDomain(Customer customer, CustomerApplicationDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));
        ArgumentNullException.ThrowIfNull(customerDto, nameof(customerDto));

        Copy(customer, customerDto);
    }

    public CustomerApplicationDto FromDomain(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));

        var customerDto = new CustomerApplicationDto();
        Copy(customer, customerDto);

        return customerDto;
    }

    public List<Customer> ToDomain(IEnumerable<CustomerApplicationDto> customerDtos) =>
        customerDtos?.Select(ToDomain).ToList() ?? [];

    public List<CustomerApplicationDto> FromDomain(IEnumerable<Customer> customers) =>
        customers?.Select(FromDomain).ToList() ?? [];

    private static void Copy(Customer customer, CustomerApplicationDto customerDto)
    {
        customerDto.Id = customer.Id.Value;

        customerDto.FirstName = customer.Name?.FirstName;
        customerDto.LastName = customer.Name?.LastName;

        if (customer.BillingAddress?.IsNotEmpty() == true)
        {
            customerDto.BillingAddressLine1 = customer.BillingAddress.AddressLine1;
            customerDto.BillingAddressLine2 = customer.BillingAddress.AddressLine2;
            customerDto.BillingCity = customer.BillingAddress.City;
            customerDto.BillingState = customer.BillingAddress.State.Abbreviation;
            customerDto.BillingZipCode = customer.BillingAddress.ZipCode.ZipCodeValue;
            customerDto.BillingCountry = customer.BillingAddress.Country;
        }

        if (customer.ShippingAddress?.IsNotEmpty() == true)
        {
            customerDto.ShippingAddressLine1 = customer.ShippingAddress.AddressLine1;
            customerDto.ShippingAddressLine2 = customer.ShippingAddress.AddressLine2;
            customerDto.ShippingCity = customer.ShippingAddress.City;
            customerDto.ShippingState = customer.ShippingAddress.State.Abbreviation;
            customerDto.ShippingZipCode = customer.ShippingAddress.ZipCode.ZipCodeValue;
            customerDto.ShippingCountry = customer.ShippingAddress.Country;
        }
    }
}