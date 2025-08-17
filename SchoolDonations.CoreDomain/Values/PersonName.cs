using DDD.Concepts.BaseTypes;
using Utilities.Extensions;

namespace SchoolDonations.CoreDomain.Values;

public record PersonName : Value
{
    #region Properties

    public string FirstName
    {
        get;
        init;
    }

    public string LastName
    {
        get;
        init;
    }

    #endregion Properties

    #region Construction

    public PersonName(PersonName name) : base(name)
    {
       FirstName = name.FirstName;
       LastName = name.LastName;
    }

    #endregion Construction

    #region Methods

    public override bool IsEmpty()
    {
        return FirstName.IsNullOrEmpty() || LastName.IsNullOrEmpty();
    }

    public override string ToString()
    {
        return $"""{FirstName} {LastName}""";
    }

    #endregion Methods
}
