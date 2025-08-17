namespace SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;

public interface ICustomerRepository
{
    #region Queries

    Task<Customer> GetByIdAsync(long id);
    Task<List<Customer>> GetAllAsync();

    #endregion Queries

    #region Commands

    Task<Lazy<Customer>> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(long id);

    #endregion Commands
}
