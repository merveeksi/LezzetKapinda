using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.ValueObjects;

[Owned]
public sealed record Email
{
    private const string Pattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    public string Value { get; init; }

    public Email(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Invalid email address");

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
    public static implicit operator string(Email email) => email.Value; // Email to string

    public static implicit operator Email(string value) => new(value); // string to Email
    
}