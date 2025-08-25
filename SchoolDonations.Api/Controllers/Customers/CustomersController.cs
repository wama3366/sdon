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
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        var addedCustomer = await CustomerService.CreateCustomerAsync(appDtoCustomer);

        return Mapper.Map<CustomerApiDto>(addedCustomer);
    }

    #endregion Commands
}