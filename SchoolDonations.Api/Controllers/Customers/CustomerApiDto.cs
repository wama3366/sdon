using SchoolDonations.API.Mapping;

namespace SchoolDonations.API.Controllers.Customers;

public class CustomerApiDto : Dto
{
    #region Properties

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string BillingAddressLine1 { get; set; }
    public string BillingAddressLine2 { get; set; }
    public string BillingCity { get; set; }
    public string BillingState { get; set; }
    public string BillingZipCode { get; set; }
    public string BillingCountry { get; set; }

    public string ShippingAddressLine1 { get; set; }
    public string ShippingAddressLine2 { get; set; }
    public string ShippingCity { get; set; }
    public string ShippingState { get; set; }
    public string ShippingCountry { get; set; }
    public string ShippingZipCode { get; set; }

    #region Meta

    internal DateTimeOffset ModifiedAt { get; set; }
    internal DateTimeOffset CreatedAt { get; set; }
    internal string ModifiedBy { get; set; }
    internal string CreatedBy { get; set; }
    internal bool IsDeleted { get; set; }
    internal long RowVersion { get; set; }

    #endregion Meta

    #endregion Properties
}