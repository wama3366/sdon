using AutoMapper;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;
using Utilities.AppDateTime;
using System.Collections.Generic;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerService : ICustomerService
{
    private IUnitOfWork UnitOfWork { get; }
    private IAppDateTime AppDateTime { get; }
    private IMapper Mapper { get; }


    #region Construction

    public CustomerService(
        IUnitOfWork unitOfWork,
        IAppDateTime appDateTime,
        IMapper mapper)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        AppDateTime = appDateTime ?? throw new ArgumentNullException(nameof(appDateTime));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion Construction

    #region Queries

    public async Task<Customer> GetCustomerByIdAsync(long customerId)
    {
        var customer = await UnitOfWork.CustomerRepository.GetByIdAsync(customerId);
        return Mapper.Map<CustomerApplicationDto>(customer);
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        var customers = await UnitOfWork.CustomerRepository.GetAllAsync();
        return Mapper.Map<List<CustomerApplicationDto>>(customers);
    }

    #endregion Queries

    #region Commands

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        var customer = Mapper.Map<Customer>(customerDto);
        // Persist
        var addResult = await UnitOfWork.CustomerRepository.AddAsync(customer);

        // Do other transactional stuff, like locally persist domain event

        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
        // Lazy.Value Must only be called after SaveChanges or SaveChangesAsync
        var addedCustomer = addResult.Value;

        return Mapper.Map<CustomerApplicationDto>(addedCustomer);
    }

    public async Task ActivateCustomer(Customer customer)
    {
        var customer = Mapper.Map<Customer>(customerDto);
        customer.Activate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeactivateCustomer(Customer customer)
    {
        var customer = Mapper.Map<Customer>(customerDto);
        customer.Deactivate(AppDateTime.UtcNow);

        // Persist
        await UnitOfWork.CustomerRepository.UpdateAsync(customer);
        // Commit transaction.
        await UnitOfWork.SaveChangesAsync();
    }

    #endregion Commands
}

