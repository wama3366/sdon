namespace SchoolDonations.ApplicationServices.Services.Customers;

public interface ICustomerService
{
    #region Queries

    Task<CustomerApplicationDto> GetCustomerByIdAsync(long customerId);
    Task<List<CustomerApplicationDto>> GetAllCustomers();

    #endregion Queries

    #region Commands

    Task<CustomerApplicationDto> CreateCustomerAsync(CustomerApplicationDto customerDto);

    Task ActivateCustomer(CustomerApplicationDto customerDto);

    Task DeactivateCustomer(CustomerApplicationDto customerDto);

    #endregion Commands
}