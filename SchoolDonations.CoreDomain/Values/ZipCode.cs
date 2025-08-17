using System.Text.RegularExpressions;
using DDD.Concepts.BaseTypes;
using Utilities.Extensions;

namespace SchoolDonations.CoreDomain.Values;

public record ZipCode : Value
{
    #region Properties

    #region Data

    public string ZipCodeValue
    {
        get;
        init => field = !IsValidZipCode(value)
            ? throw new ArgumentException($"{nameof(ZipCodeValue)} is not valid.")
            : value;
    }

    #endregion Data

    // Valid formats: 12345 or 12345-6789
    private static readonly string zipCodePattern = @"^\d{5}(-\d{4})?$";
    private static readonly Regex zipCodeRegex = new(zipCodePattern, RegexOptions.Compiled);

    #endregion Properties

    #region Construction

    public ZipCode() { }

    public ZipCode(string zipCode)
    {
        ZipCodeValue = zipCode;
    }

    public ZipCode(ZipCode zipCode) : base(zipCode)
    {
        ZipCodeValue = zipCode.ZipCodeValue;
    }

    #endregion Construction

    #region Methods

    private static bool IsValidZipCode(string zipCode)
        => zipCode.IsNullOrEmpty() || zipCodeRegex.IsMatch(zipCode);

    public override bool IsEmpty()
    {
        return ZipCodeValue.IsNullOrEmpty();
    }

    public override string ToString()
    {
        return $"""{ZipCodeValue}""";
    }

    #endregion Methods
}
