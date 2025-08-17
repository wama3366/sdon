using DDD.Concepts.BaseTypes;
using Utilities.Extensions;

namespace SchoolDonations.CoreDomain.Values;

public record Address : Value
{
    #region Properties

    public string AddressLine1
    {
        get;
        init;
    }

    public string AddressLine2
    {
        get;
        init;
    }

    public string City
    {
        get;
        init;
    }

    public UsState State
    {
        get;
        init;
    }

    public ZipCode ZipCode
    {
        get;
        init;
    }

    public string Country
    {
        get;
        init;
    }

    #endregion Properties

    #region Construction

    public Address(Address address) : base(address)
    {
        AddressLine1 = address.AddressLine1;
        AddressLine2 = address.AddressLine2;
        City = address.City;
        State = address.State;
        ZipCode = address.ZipCode;
        Country = address.Country;
    }

    #endregion Construction

    #region Methods

    public override bool IsEmpty()
    {
        return AddressLine1.IsNullOrEmpty() || City.IsNullOrEmpty() || (State?.IsNullOrEmpty() ?? true) || ZipCode.IsNullOrEmpty() || Country.IsNullOrEmpty();
    }

    public override string ToString()
    {
        return $"""
                {AddressLine1},
                {AddressLine1},
                {City}, {State} {ZipCode}, 
                {Country}
                """;
    }

    #endregion Methods
}
