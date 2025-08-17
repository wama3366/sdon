using DDD.Concepts.BaseTypes;
using Utilities.Extensions;

namespace SchoolDonations.CoreDomain.Values;

public record UsState : Value
{
    #region Properties

    public string Abbreviation
    {
        get;
        init;
    }

    public string FullName
    {
        get;
        init;
    }

    #endregion Properties

    #region Construction

    public UsState(UsState state) : base(state)
    {
        Abbreviation = state.Abbreviation;
        FullName = state.FullName;
    }

    #endregion Construction

    #region Methods

    public override bool IsEmpty()
    {
        return Abbreviation.IsNullOrEmpty() || FullName.IsNullOrEmpty();
    }

    public override string ToString()
    {
        return $"{FullName} ({Abbreviation})";
    }

    public static UsState GetByAbbreviation(string abbreviation)
    {
        return USStates.FirstOrDefault(state => state.Abbreviation.Equals(abbreviation, StringComparison.OrdinalIgnoreCase));
    }

    public static UsState GetByFullName(string fullName)
    {
        return USStates.FirstOrDefault(state => state.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));
    }

    #endregion Methods

    public static readonly List<UsState> USStates =
    [
        new UsState { Abbreviation = "AL", FullName = "Alabama" },
        new UsState { Abbreviation = "AK", FullName = "Alaska" },
        new UsState { Abbreviation = "AZ", FullName = "Arizona" },
        new UsState { Abbreviation = "AR", FullName = "Arkansas" },
        new UsState { Abbreviation = "CA", FullName = "California" },
        new UsState { Abbreviation = "CO", FullName = "Colorado" },
        new UsState { Abbreviation = "CT", FullName = "Connecticut" },
        new UsState { Abbreviation = "DE", FullName = "Delaware" },
        new UsState { Abbreviation = "FL", FullName = "Florida" },
        new UsState { Abbreviation = "GA", FullName = "Georgia" },
        new UsState { Abbreviation = "HI", FullName = "Hawaii" },
        new UsState { Abbreviation = "ID", FullName = "Idaho" },
        new UsState { Abbreviation = "IL", FullName = "Illinois" },
        new UsState { Abbreviation = "IN", FullName = "Indiana" },
        new UsState { Abbreviation = "IA", FullName = "Iowa" },
        new UsState { Abbreviation = "KS", FullName = "Kansas" },
        new UsState { Abbreviation = "KY", FullName = "Kentucky" },
        new UsState { Abbreviation = "LA", FullName = "Louisiana" },
        new UsState { Abbreviation = "ME", FullName = "Maine" },
        new UsState { Abbreviation = "MD", FullName = "Maryland" },
        new UsState { Abbreviation = "MA", FullName = "Massachusetts" },
        new UsState { Abbreviation = "MI", FullName = "Michigan" },
        new UsState { Abbreviation = "MN", FullName = "Minnesota" },
        new UsState { Abbreviation = "MS", FullName = "Mississippi" },
        new UsState { Abbreviation = "MO", FullName = "Missouri" },
        new UsState { Abbreviation = "MT", FullName = "Montana" },
        new UsState { Abbreviation = "NE", FullName = "Nebraska" },
        new UsState { Abbreviation = "NV", FullName = "Nevada" },
        new UsState { Abbreviation = "NH", FullName = "New Hampshire" },
        new UsState { Abbreviation = "NJ", FullName = "New Jersey" },
        new UsState { Abbreviation = "NM", FullName = "New Mexico" },
        new UsState { Abbreviation = "NY", FullName = "New York" },
        new UsState { Abbreviation = "NC", FullName = "North Carolina" },
        new UsState { Abbreviation = "ND", FullName = "North Dakota" },
        new UsState { Abbreviation = "OH", FullName = "Ohio" },
        new UsState { Abbreviation = "OK", FullName = "Oklahoma" },
        new UsState { Abbreviation = "OR", FullName = "Oregon" },
        new UsState { Abbreviation = "PA", FullName = "Pennsylvania" },
        new UsState { Abbreviation = "RI", FullName = "Rhode Island" },
        new UsState { Abbreviation = "SC", FullName = "South Carolina" },
        new UsState { Abbreviation = "SD", FullName = "South Dakota" },
        new UsState { Abbreviation = "TN", FullName = "Tennessee" },
        new UsState { Abbreviation = "TX", FullName = "Texas" },
        new UsState { Abbreviation = "UT", FullName = "Utah" },
        new UsState { Abbreviation = "VT", FullName = "Vermont" },
        new UsState { Abbreviation = "VA", FullName = "Virginia" },
        new UsState { Abbreviation = "WA", FullName = "Washington" },
        new UsState { Abbreviation = "WV", FullName = "West Virginia" },
        new UsState { Abbreviation = "WI", FullName = "Wisconsin" },
        new UsState { Abbreviation = "WY", FullName = "Wyoming" }
    ];
}
