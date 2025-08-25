using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Persistence.Concepts;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;
using SchoolDonations.EFCore.Customers;

namespace SchoolDonations.EFCore.Tests;

public class CustomerRepositoryTests
{
    private readonly AppDbContext _appDbContext;
    private readonly CustomerRepository _customerRepository;

    public CustomerRepositoryTests()
    {
        var dbSettings = new Mock<IOptionsSnapshot<DbSettings>>();
        dbSettings.Setup(x => x.Value).Returns(new DbSettings());

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options, dbSettings.Object);
        _customerRepository = new CustomerRepository(_appDbContext);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer()
    {
        var customer = AddCustomer();
        var result = await _customerRepository.GetByIdAsync(customer.Id.Value);
        Assert.NotNull(result);
        Assert.Equal("John", result.Name.FirstName);
        Assert.Equal("Doe", result.Name.LastName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        AddCustomers();
        var customers = await _customerRepository.GetAllAsync();
        Assert.Equal(2, customers.Count);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        var customer = new Customer(new CustomerId(0))
        {
            Name = new PersonName { FirstName = "John", LastName = "Doe" }
        };

        var result = await _customerRepository.AddAsync(customer);
        await _appDbContext.SaveChangesAsync();

        var created = await _appDbContext.Customers.FindAsync(customer.Id.Value);
        Assert.NotNull(result);
        Assert.NotNull(created);
        Assert.Equal("John", created.Name.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyCustomer()
    {
        var customer = AddCustomer();
        customer.Name = new PersonName { FirstName = "John2", LastName = "Doe2" };

        await _customerRepository.UpdateAsync(customer);
        await _appDbContext.SaveChangesAsync();

        var updated = await _appDbContext.Customers.FindAsync(customer.Id.Value);
        Assert.Equal("John2", updated.Name.FirstName);
        Assert.Equal("Doe2", updated.Name.LastName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomer()
    {
        var customer = AddCustomer();

        await _customerRepository.DeleteAsync(customer.Id.Value);
        await _appDbContext.SaveChangesAsync();

        var deleted = await _appDbContext.Customers.FindAsync(customer.Id.Value);
        Assert.Null(deleted);
    }

    private Customer AddCustomer()
    {
        var customer = new Customer(new CustomerId(0))
        {
            Name = new PersonName { FirstName = "John", LastName = "Doe" }
        };
        _appDbContext.Customers.Add(customer);
        _appDbContext.SaveChanges();
        return customer;
    }

    private void AddCustomers()
    {
        var customer1 = new Customer(new CustomerId(0))
        {
            Name = new PersonName { FirstName = "John", LastName = "Doe" }
        };
        var customer2 = new Customer(new CustomerId(0))
        {
            Name = new PersonName { FirstName = "Jane", LastName = "Doe" }
        };
        _appDbContext.Customers.AddRange(customer1, customer2);
        _appDbContext.SaveChanges();
    }
}
