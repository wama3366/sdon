using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;
using Utilities.AppDateTime;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerService : ICustomerService
{
    private IUnitOfWork UnitOfWork { get; }
    private IAppDateTime AppDateTime { get; }
    private IApplicationMapper<Customer, CustomerApplicationDto> CustomerMapper { get; }

    #region Construction

    public CustomerService(
        IUnitOfWork unitOfWork,
        IAppDateTime appDateTime,
        IApplicationMapper<Customer, CustomerApplicationDto> customerMapper)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        AppDateTime = appDateTime ?? throw new ArgumentNullException(nameof(appDateTime));
        CustomerMapper = customerMapper ?? throw new ArgumentNullException(nameof(customerMapper));
    }

    #endregion Construction

    #region Queries

    public async Task<CustomerApplicationDto> GetCustomerByIdAsync(long customerId)
    {
        var customer = await UnitOfWork.CustomerRepository.GetByIdAsync(customerId);
        return CustomerMapper.FromDomain(customer);
    }

    public async Task<List<CustomerApplicationDto>> GetAllCustomers()
    {
        var customers = await UnitOfWork.CustomerRepository.GetAllAsync();
        return CustomerMapper.FromDomain(customers);
    }

    #endregion Queries

    #region Commands

    public async Task<CustomerApplicationDto> CreateCustomerAsync(CustomerApplicationDto customerDto)
    {
        var customer = CustomerMapper.ToDomain(customerDto);

        // Persist
        var addResult = await UnitOfWork.CustomerRepository.AddAsync(customer);

        // Do other transactional stuff, like locally persist domain event

        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
        // Lazy.Value Must only be called after SaveChanges or SaveChangesAsync
        var addedCustomer = addResult.Value;

        return CustomerMapper.FromDomain(addedCustomer);
    }

    public async Task ActivateCustomer(CustomerApplicationDto customerDto)
    {
        var customer = CustomerMapper.ToDomain(customerDto);
        customer.Activate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeactivateCustomer(CustomerApplicationDto customerDto)
    {
        var customer = CustomerMapper.ToDomain(customerDto);
        customer.Deactivate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    #endregion Commands
}
