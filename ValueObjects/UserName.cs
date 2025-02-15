using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.ValueObjects;

[Owned]
public sealed class UserName : IEquatable<UserName>
{
    public string Value { get; private set; }

    private UserName() { }
    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("User name cannot be empty");

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("User name must be between 3 and 50 characters.");

        Value = value;
    }

    public static UserName Create(string value)
    {
        return new UserName(value);
    }

    // EF Core için gerekli conversion metodu
    public override string ToString() => Value;

    // Value object için eşitlik kontrolü
    public override bool Equals(object? obj) => obj is UserName other && Value == other.Value;

    public bool Equals(UserName? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
