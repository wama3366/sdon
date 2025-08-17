using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDonations.API.Mapping;
using SchoolDonations.ApplicationServices.Services.Customers;

namespace SchoolDonations.API.Controllers.Customers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private IApiMapper<CustomerApplicationDto, CustomerApiDto> CustomerMapper { get; }
    private ICustomerService CustomerService { get; }
    private ILogger<CustomersController> Logger { get; }

    #region Contruction

    public CustomersController(
        IApiMapper<CustomerApplicationDto, CustomerApiDto> customerMapper,
        ICustomerService customerService,
        ILogger<CustomersController> logger)
    {
        CustomerMapper = customerMapper ?? throw new ArgumentNullException(nameof(customerMapper));
        CustomerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion Contruction

    #region Queries

    [HttpGet("{customerId:long}")]
    public async Task<IActionResult> GetCustomerByIdAsync(long customerId)
    {
        var customer = await CustomerService.GetCustomerByIdAsync(customerId);
        return Ok(CustomerMapper.FromAppDto(customer));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await CustomerService.GetAllCustomers();
        return Ok(CustomerMapper.FromAppDto(customers));
    }

    #endregion Queries

    #region Commands

    [HttpPost]
    public async Task<CustomerApiDto> CreateCustomerAsync(CustomerApiDto customer)
    {
        var appDtoCustomer = CustomerMapper.ToAppDto(customer);

        var addedCustomer = await CustomerService.CreateCustomerAsync(appDtoCustomer);

        return CustomerMapper.FromAppDto(addedCustomer);
    }

    #endregion Commands
}