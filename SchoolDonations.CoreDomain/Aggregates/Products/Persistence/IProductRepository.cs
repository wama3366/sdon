namespace SchoolDonations.CoreDomain.Aggregates.Products.Persistence;

public interface IProductRepository
{
    #region Queries

    Task<Product> GetByIdAsync(long id);
    Task<List<Product>> GetAllAsync();

    #endregion Queries

    #region Commands

    Task<Lazy<Product>> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task DeleteAsync(long id);

    #endregion Commands
}
