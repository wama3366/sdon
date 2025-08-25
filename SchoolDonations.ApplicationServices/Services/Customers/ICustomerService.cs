using SchoolDonations.CoreDomain.Aggregates.Customers;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public interface ICustomerService
{
    #region Queries

    Task<Customer> GetCustomerByIdAsync(long customerId);
    Task<List<Customer>> GetAllCustomers();

    #endregion Queries

    #region Commands

    Task<Customer> CreateCustomerAsync(Customer customer);

    Task ActivateCustomer(Customer customer);

    Task DeactivateCustomer(Customer customer);

    #endregion Commands
}