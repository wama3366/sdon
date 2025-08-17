using DDD.Concepts.Interfaces;

namespace SchoolDonations.CoreDomain.Aggregates.Products.Events;

public class ProductEventData : IDomainEventData
{
    #region Construction

    public ProductEventData(Product product) => _product = product;

    #endregion Construction

    #region Properties

    private readonly Product _product;

    #endregion Properties

    public string GetKeyValueString()
    {
        return $"Id:{_product.Id.Value}";
    }
}
