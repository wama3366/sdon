using Microsoft.EntityFrameworkCore;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;

namespace SchoolDonations.EFCore.Customers;

public class CustomerRepository : ICustomerRepository
{
    private DbSet<Customer> Customers { get; }

    public CustomerRepository(AppDbContext appDbContext)
    {
        Customers = appDbContext?.Customers ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Customer> GetByIdAsync(long id)
        => await Customers.FindAsync(id);

    public async Task<List<Customer>> GetAllAsync()
        => await Customers.ToListAsync();

    public async Task<Lazy<Customer>> AddAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        var entry = await Customers.AddAsync(customer);
        return new Lazy<Customer>(() => entry.Entity);
    }

    public Task UpdateAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        Customers.Update(customer);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await Customers.FindAsync(id);
        if (entity != null)
        {
            Customers.Remove(entity);
        }
    }
}
