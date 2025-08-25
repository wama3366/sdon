using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;
using Utilities.AppDateTime;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerService : ICustomerService
{
    private IUnitOfWork UnitOfWork { get; }
    private IAppDateTime AppDateTime { get; }

    #region Construction

    public CustomerService(
        IUnitOfWork unitOfWork,
        IAppDateTime appDateTime)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        AppDateTime = appDateTime ?? throw new ArgumentNullException(nameof(appDateTime));
    }

    #endregion Construction

    #region Queries

    public async Task<Customer> GetCustomerByIdAsync(long customerId)
    {
        var customer = await UnitOfWork.CustomerRepository.GetByIdAsync(customerId);
        return customer;
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        var customers = await UnitOfWork.CustomerRepository.GetAllAsync();
        return customers;
    }

    #endregion Queries

    #region Commands

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        // Persist
        var addResult = await UnitOfWork.CustomerRepository.AddAsync(customer);

        // Do other transactional stuff, like locally persist domain event

        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
        // Lazy.Value Must only be called after SaveChanges or SaveChangesAsync
        var addedCustomer = addResult.Value;

        return addedCustomer;
    }

    public async Task ActivateCustomer(Customer customer)
    {
        customer.Activate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeactivateCustomer(Customer customer)
    {
        customer.Deactivate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    #endregion Commands
}
