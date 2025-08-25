using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDonations.ApplicationServices.Services.Customers;
using System.Collections.Generic;

namespace SchoolDonations.API.Controllers.Customers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private IMapper Mapper { get; }
    private ICustomerService CustomerService { get; }
    private ILogger<CustomersController> Logger { get; }

    #region Contruction

    public CustomersController(
        IMapper mapper,
        ICustomerService customerService,
        ILogger<CustomersController> logger)
    {
        CustomerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion Contruction

    #region Queries

    [HttpGet("{customerId:long}")]
    public async Task<IActionResult> GetCustomerByIdAsync(long customerId)
    {
        var customer = await CustomerService.GetCustomerByIdAsync(customerId);
        return Ok(Mapper.Map<CustomerApiDto>(customer));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await CustomerService.GetAllCustomers();
        return Ok(Mapper.Map<List<CustomerApiDto>>(customers));
    }

    #endregion Queries

    #region Commands

    [HttpPost]
    public async Task<CustomerApiDto> CreateCustomerAsync(CustomerApiDto customer)
    {
        var appDtoCustomer = Mapper.Map<CustomerApplicationDto>(customer);
        var addedCustomer = await CustomerService.CreateCustomerAsync(domainCustomer);

        return Mapper.Map<CustomerApiDto>(addedCustomer);
    }

    #endregion Commands

    #region Mapping

    private static Customer ToDomain(CustomerApiDto customerDto)
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

    private static CustomerApiDto FromDomain(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));

        var dto = new CustomerApiDto
        {
            Id = customer.Id.Value,
            FirstName = customer.Name?.FirstName,
            LastName = customer.Name?.LastName,
        };

        if (customer.BillingAddress?.IsNotEmpty() == true)
        {
            dto.BillingAddressLine1 = customer.BillingAddress.AddressLine1;
            dto.BillingAddressLine2 = customer.BillingAddress.AddressLine2;
            dto.BillingCity = customer.BillingAddress.City;
            dto.BillingState = customer.BillingAddress.State.Abbreviation;
            dto.BillingZipCode = customer.BillingAddress.ZipCode.ZipCodeValue;
            dto.BillingCountry = customer.BillingAddress.Country;
        }

        if (customer.ShippingAddress?.IsNotEmpty() == true)
        {
            dto.ShippingAddressLine1 = customer.ShippingAddress.AddressLine1;
            dto.ShippingAddressLine2 = customer.ShippingAddress.AddressLine2;
            dto.ShippingCity = customer.ShippingAddress.City;
            dto.ShippingState = customer.ShippingAddress.State.Abbreviation;
            dto.ShippingZipCode = customer.ShippingAddress.ZipCode.ZipCodeValue;
            dto.ShippingCountry = customer.ShippingAddress.Country;
        }

        return dto;
    }

    #endregion Mapping
}