using Microsoft.EntityFrameworkCore;
using Persistence.Concepts;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;
using Utilities.AppDateTime;

namespace SchoolDonations.EFCore.Customers;

public class CustomerRepository : ICustomerRepository
{
    private DbSet<CustomerPersistenceDto> Customers { get; }
    private IAppDateTime AppDateTime { get; }
    private IPersistenceMapper<Customer, CustomerPersistenceDto> CustomerMapper { get; }

    #region Construction

    public CustomerRepository(
        AppDbContext appDbContext,
        IAppDateTime appDateTime,
        IPersistenceMapper<Customer, CustomerPersistenceDto> customerMapper)
    {
        Customers = appDbContext?.Customers ?? throw new ArgumentNullException(nameof(appDbContext));
        AppDateTime = appDateTime ?? throw new ArgumentNullException(nameof(appDateTime));
        CustomerMapper = customerMapper ?? throw new ArgumentNullException(nameof(customerMapper));
    }

    #endregion Construction

    #region Queries

    public async Task<Customer> GetByIdAsync(long id)
    {
        var customerDto = await Customers.FindAsync(id);
        return CustomerMapper.ToDomain(customerDto);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var customerDtos = await Customers.ToListAsync();
        return CustomerMapper.ToDomain(customerDtos);
    }

    #endregion Queries

    #region Commands

    public async Task<Lazy<Customer>> AddAsync(Customer customer)
    {
        var customerDto = CustomerMapper.FromDomain(customer);

        customerDto.RowVersion = 1;
        // TODO: How to fill this data?
        customerDto.ModifiedAt = AppDateTime.UtcNow;
        customerDto.CreatedAt = AppDateTime.UtcNow;
        customerDto.ModifiedBy = "System";
        customerDto.CreatedBy = "System";

        var createdCustomerDto = await Customers.AddAsync(customerDto);

        // Must delay mapping until SaveChanges is called.
        return CustomerMapper.ToDomainLazy(createdCustomerDto.Entity);
    }

    public async Task UpdateAsync(Customer customer)
    {
        var customerDto = await Customers.FindAsync(customer.Id.Value);
        if (customerDto == null)
        {
            return;
        }

        // This will automatically track changes on customerDto
        CustomerMapper.CopyFromDomain(customer, customerDto);

        customerDto.RowVersion += 1;
        // TODO: How to fill this data?
        customerDto.ModifiedAt = AppDateTime.UtcNow;
        customerDto.ModifiedBy = "System";
    }

    public async Task DeleteAsync(long id)
    {
        var customerDto = await Customers.FindAsync(id);
        if (customerDto != null)
        {
            Customers.Remove(customerDto);
        }
    }

    #endregion Commands
}
