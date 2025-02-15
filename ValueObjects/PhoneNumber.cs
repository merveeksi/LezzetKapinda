using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.ValueObjects;

[Owned]

public sealed record PhoneNumber
{
    private const string Pattern = @"^(\+90[\s-]?)?0?([0-9]{3})[\s-]?([0-9]{3})[\s-]?([0-9]{2})[\s-]?([0-9]{2})$";
    public string Value { get; init; }

    public PhoneNumber(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Invalid phone number");

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        if (!Regex.IsMatch(value, Pattern))
            return false;

        return true;
    }

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value; // PhoneNumber to string

    public static implicit operator PhoneNumber(string value) => new(value); // string to PhoneNumber
}