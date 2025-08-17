using DDD.Concepts.BaseTypes;
using SchoolDonations.CoreDomain.Aggregates.Products.Events;

namespace SchoolDonations.CoreDomain.Aggregates.Products;

public readonly record struct ProductId(long Value) : IIdentityKey;

public class Product : Entity<ProductId>
{
    // Data
    public required string Name { get; set; }

    #region Construction

    internal Product() : base(new ProductId(0))
    {

    }

    internal Product(ProductId productId) : base(productId)
    {
    }

    internal Product(Product product) : base(product)
    {
        Name = product.Name;
    }

    #endregion Construction

    #region Methods

    public static Product Add(Product product, DateTimeOffset occuredOn)
    {
        var @event = new ProductAdded(product, occuredOn);
        product.AddDomainEvent(@event);
        return product;
    }

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}