using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Persistence.Concepts;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;
using SchoolDonations.EFCore.Customers;
using Utilities.AppDateTime;

namespace SchoolDonations.EFCore.Tests;

public class CustomerRepositoryTests
{
    private readonly AppDbContext _appDbContext;
    private readonly CustomerRepository _customerRepository;
    private readonly AppDateTime _appDateTime;

    // TODO: Add tests to check for ModifiedAt and CreatedAt and ModifiedBy and CreatedBy and RowVersion
    public CustomerRepositoryTests()
    {
        var dbSettings = new Mock<IOptionsSnapshot<DbSettings>>();
        dbSettings.Setup(x => x.Value).Returns(new DbSettings());

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options, dbSettings.Object);
        _customerRepository = new CustomerRepository(_appDbContext, new AppDateTime(), new CustomerPersistenceMapper());
        _appDateTime = new();
    }

    #region Queries

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer()
    {
        // Arrange
        var customerDto = AddCustomer();

        // Act
        var result = await _customerRepository.GetByIdAsync(customerDto.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Name.FirstName);
        Assert.Equal("Doe", result.Name.LastName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        // Arrange
        AddCustomers();

        // Act
        var customers = await _customerRepository.GetAllAsync();

        // Assert
        Assert.Equal(2, customers.Count);
    }

    #endregion Queries

    #region Commands

    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        // Arrange
        var customer = new Customer(new CustomerId(1)) { Name = new PersonName { FirstName = "John", LastName = "Doe" } };

        // Act
        var result = await _customerRepository.AddAsync(customer);

        // Assert
        await _appDbContext.SaveChangesAsync();
        customer = result.Value;
        Assert.NotNull(result);
        Assert.Equal("John", customer.Name.FirstName);
        Assert.Equal("Doe", customer.Name.LastName);
        Assert.Equal(1, _appDbContext.Customers.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyCustomer()
    {
        // Arrange
        var customerDto = AddCustomer();

        customerDto.FirstName = "John2";
        customerDto.LastName = "Doe2";

        // Act
        await _customerRepository.UpdateAsync(new CustomerPersistenceMapper().ToDomain(customerDto));

        // Assert
        var updated = await _appDbContext.Customers.FindAsync(customerDto.Id);
        Assert.Equal("John2", updated?.FirstName);
        Assert.Equal("Doe2", updated?.LastName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomer()
    {
        // Arrange
        var customerDto = AddCustomer();

        // Act
        await _customerRepository.DeleteAsync(customerDto.Id);

        // Assert
        await _appDbContext.SaveChangesAsync();
        var deleted = await _appDbContext.Customers.FindAsync(customerDto.Id);
        Assert.Null(deleted);
    }

    #endregion Commands

    #region Helpers

    private CustomerPersistenceDto AddCustomer()
    {
        var customerDto = new CustomerPersistenceDto
        {
            FirstName = "John",
            LastName = "Doe",
            ModifiedAt = _appDateTime.UtcNow,
            ModifiedBy = "User",
            CreatedAt = _appDateTime.UtcNow,
            CreatedBy = "User",
            RowVersion = new Random().Next()
        };
        _appDbContext.Customers.Add(customerDto);
        _appDbContext.SaveChanges();
        return customerDto;
    }

    private void AddCustomers()
    {
        var customerDto1 = new CustomerPersistenceDto
        {
            FirstName = "John",
            LastName = "Doe",
            ModifiedAt = _appDateTime.UtcNow,
            ModifiedBy = "User",
            CreatedAt = _appDateTime.UtcNow,
            CreatedBy = "User",
            RowVersion = new Random().Next()
        };
        var customerDto2 = new CustomerPersistenceDto
        {
            FirstName = "John2",
            LastName = "Doe",
            ModifiedAt = _appDateTime.UtcNow,
            ModifiedBy = "User2",
            CreatedAt = _appDateTime.UtcNow,
            CreatedBy = "User2",
            RowVersion = new Random().Next()
        };

        _appDbContext.Customers.AddRange([customerDto1, customerDto2]);
        _appDbContext.SaveChanges();
    }

    #endregion Helpers
}
