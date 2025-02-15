using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.ValueObjects;

[Owned] // ✅ EF Core'un bunu yönetmesini sağlıyoruz
public sealed record Password
{
    private const int MinLength = 8;
    private const int MaxLength = 100;
    private const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

    public string Value { get; init; } // ✅ Şifre burada hashlenmiş olarak saklanacak

    public Password(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Password cannot be null or empty");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException($"Password must be between {MinLength} and {MaxLength} characters");

        if (!Regex.IsMatch(value, Pattern))
            throw new ArgumentException("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

        Value = HashPassword(value); // ✅ Şifre hashlenerek saklanıyor
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool Verify(string plainPassword)
    {
        return HashPassword(plainPassword) == Value;
    }

    public static implicit operator string(Password password) => password.Value;
    public static implicit operator Password(string value) => new(value);
}