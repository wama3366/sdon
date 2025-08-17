using Persistence.Concepts;

namespace SchoolDonations.EFCore.Customers;

public class CustomerPersistenceDto : Dto
{
    #region Properties

    internal string FirstName { get; set; }
    internal string LastName { get; set; }

    internal string BillingAddressLine1 { get; set; }
    internal string BillingAddressLine2 { get; set; }
    internal string BillingCity { get; set; }
    internal string BillingState { get; set; }
    internal string BillingZipCode{ get; set; }
    internal string BillingCountry { get; set; }

    internal string ShippingAddressLine1 { get; set; }
    internal string ShippingAddressLine2 { get; set; }
    internal string ShippingCity { get; set; }
    internal string ShippingState { get; set; }
    internal string ShippingCountry { get; set; }
    internal string ShippingZipCode{ get; set; }

    #region Meta

    internal DateTimeOffset ModifiedAt{ get; set; }
    internal DateTimeOffset CreatedAt{ get; set; }
    internal string ModifiedBy{ get; set; }
    internal string CreatedBy{ get; set; }
    internal bool IsDeleted{ get; set; }
    internal long RowVersion{ get; set; }

    #endregion Meta

    #endregion Properties
}
