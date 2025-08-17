namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerApplicationDto : Dto
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

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string ModifiedBy { get; set; }
    public string CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public long RowVersion { get; set; }

    #endregion Meta

    #endregion Properties
}