using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Persistence.Concepts;
using SchoolDonations.ApplicationServices.Services.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.EFCore;
using SchoolDonations.EFCore.Customers;
using SchoolDonations.EFCore.DomainEvents;
using Utilities.AppDateTime;

namespace SchoolDonations.ApplicationServices.Tests;

// TODO: These tests are commented to allow the pipelines to succeed. These tests need an actual database to work.

public class CustomerServiceTests
{
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        var dbSettings = new Mock<IOptionsSnapshot<DbSettings>>();
        dbSettings.Setup(x => x.Value).Returns(new DbSettings()
        {
            Host = "localhost",
            Port = 5432,
            Database = "school_donations_test",
            Username = "school_donations_test_user",
            Password = "@NewPass01"
        });

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var appDbContext = new AppDbContext(optionsBuilder.Options, dbSettings.Object);
        var unitOfWork = new UnitOfWork(
            appDbContext,
            new CustomerRepository(appDbContext),
            new DomainEventRepository(appDbContext, new AppDateTime(), new DomainEventPersistenceMapper()));
        IApplicationMapper<Customer, CustomerApplicationDto> customerMapper = new CustomerApplicationMapper();
        _customerService = new CustomerService(unitOfWork, new AppDateTime(), customerMapper);
    }

    // [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomer()
    {
        // Arrange
        var customerDto = new CustomerApplicationDto()
        {
            FirstName = "John",
            LastName = "Doe",
            BillingAddressLine1 = "123 Main Street",
            BillingAddressLine2 = "Apt #2",
            BillingCity = "Raleigh",
            BillingState = "NC",
            BillingZipCode = "12345",
            BillingCountry = "USA",
            ShippingAddressLine1 = "321 Main Street",
            ShippingAddressLine2 = "Apt #3",
            ShippingCity = "Charlotte",
            ShippingState = "NC",
            ShippingZipCode = "54321",
            ShippingCountry = "USA",
        };
        var addedCustomer = await _customerService.CreateCustomerAsync(customerDto);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(addedCustomer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addedCustomer.Id, result.Id);
        Assert.Equal(addedCustomer.FirstName, result.FirstName);
        Assert.Equal(addedCustomer.LastName, result.LastName);
    }

    // [Fact]
    public async Task CreateCustomerAsync_ShouldCreateCustomer()
    {
        // Arrange
        var customerDto = new CustomerApplicationDto()
        {
            FirstName = "John",
            LastName = "Doe",
            BillingAddressLine1 = "123 Main Street",
            BillingAddressLine2 = "Apt #2",
            BillingCity = "Raleigh",
            BillingState = "NC",
            BillingZipCode = "12345",
            BillingCountry = "USA",
            ShippingAddressLine1 = "321 Main Street",
            ShippingAddressLine2 = "Apt #3",
            ShippingCity = "Charlotte",
            ShippingState = "NC",
            ShippingZipCode = "54321",
            ShippingCountry = "USA",
        };

        // Act
        var result = await _customerService.CreateCustomerAsync(customerDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }
}