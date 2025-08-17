using SchoolDonations.API.Mapping;
using SchoolDonations.ApplicationServices.Services.Customers;

namespace SchoolDonations.API.Controllers.Customers;

internal class CustomerApiMapper : IApiMapper<CustomerApplicationDto, CustomerApiDto>
{
    public CustomerApplicationDto ToAppDto(CustomerApiDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customerDto, nameof(customerDto));

        var customer = new CustomerApplicationDto()
        {
            Id = customerDto.Id,
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            BillingAddressLine1 = customerDto.BillingAddressLine1,
            BillingAddressLine2 = customerDto.BillingAddressLine2,
            BillingCity = customerDto.BillingCity,
            BillingState = customerDto.BillingState,
            BillingZipCode = customerDto.BillingZipCode,
            BillingCountry = customerDto.BillingCountry,
            ShippingAddressLine1 = customerDto.ShippingAddressLine1,
            ShippingAddressLine2 = customerDto.ShippingAddressLine2,
            ShippingCity = customerDto.ShippingCity,
            ShippingState = customerDto.ShippingState,
            ShippingZipCode = customerDto.ShippingZipCode,
            ShippingCountry = customerDto.ShippingCountry,
        };

        return customer;
    }

    public void CopyFromAppDto(CustomerApplicationDto customer, CustomerApiDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));
        ArgumentNullException.ThrowIfNull(customerDto, nameof(customerDto));

        Copy(customer, customerDto);
    }

    public CustomerApiDto FromAppDto(CustomerApplicationDto customer)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));

        var customerDto = new CustomerApiDto();
        Copy(customer, customerDto);

        return customerDto;
    }

    public List<CustomerApplicationDto> ToAppDto(IEnumerable<CustomerApiDto> customerDtos) =>
        customerDtos?.Select(ToAppDto).ToList() ?? [];

    public List<CustomerApiDto> FromAppDto(IEnumerable<CustomerApplicationDto> customers) =>
        customers?.Select(FromAppDto).ToList() ?? [];

    private static void Copy(CustomerApplicationDto customer, CustomerApiDto customerDto)
    {
        customerDto.Id = customer.Id;

        customerDto.FirstName = customer.FirstName;
        customerDto.LastName = customer.LastName;

        customerDto.BillingAddressLine1 = customer.BillingAddressLine1;
        customerDto.BillingAddressLine2 = customer.BillingAddressLine2;
        customerDto.BillingCity = customer.BillingCity;
        customerDto.BillingState = customer.BillingState;
        customerDto.BillingZipCode = customer.BillingZipCode;
        customerDto.BillingCountry = customer.BillingCountry;

        customerDto.ShippingAddressLine1 = customer.ShippingAddressLine1;
        customerDto.ShippingAddressLine2 = customer.ShippingAddressLine2;
        customerDto.ShippingCity = customer.ShippingCity;
        customerDto.ShippingState = customer.ShippingState;
        customerDto.ShippingZipCode = customer.ShippingZipCode;
        customerDto.ShippingCountry = customer.ShippingCountry;
    }
}