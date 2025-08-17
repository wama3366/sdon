using System.Reflection;

namespace DDD.Concepts.BaseTypes;

public abstract record Value
{
    #region Construction

    public Value(Value value)
    {
        if (value == null)
        {
            throw new ArgumentException("Value cannot be null.", nameof(value));
        }
    }

    #endregion Construction

    #region Methods

    public bool IsNotEmpty()
    {
        return !IsEmpty();
    }

    public abstract bool IsEmpty();

    public override string ToString()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return string.Join(Environment.NewLine, properties.Select(p => $"{p.Name}: {p.GetValue(this)}"));
    }

    #endregion Methods
}

public static class ValueExtensions
{
    public static bool IsNullOrEmpty(this Value value)
    {
        return value is null || value.IsEmpty();
    }

    public static bool IsNotNullNorEmpty(this Value value)
    {
        return !IsNullOrEmpty(value);
    }
}
