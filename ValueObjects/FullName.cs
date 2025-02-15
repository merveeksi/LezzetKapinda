using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace LezzetKapinda.ValueObjects;

 [Owned]
    public sealed record FullName
    {
        private const string Pattern = @"^[\p{L}' -]+$"; // Unicode harflerini destekliyor
        private const int MinLength = 1;
        private const int MaxLength = 100;

        public string FirstName { get; init; }
        public string LastName { get; init; }

        public FullName(string firstName, string lastName)
        {
            // Null veya boşlukları temizle
            firstName = firstName?.Trim();
            lastName = lastName?.Trim();

            // LOG: Değerleri yazdıralım
            Console.WriteLine($"FullName Constructor - FirstName: '{firstName}', LastName: '{lastName}'");

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty or null.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty or null.");

            if (!IsValid(firstName))
                throw new ArgumentException($"Invalid first name: '{firstName}'");

            if (!IsValid(lastName))
                throw new ArgumentException($"Invalid last name: '{lastName}'");

            FirstName = firstName;
            LastName = lastName;
        }

        public static bool IsValid(string value)
        {
            return !string.IsNullOrWhiteSpace(value) 
                   && Regex.IsMatch(value, Pattern) 
                   && value.Length >= MinLength 
                   && value.Length <= MaxLength;
        }

        public static FullName Create(string value)
        {
            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2) // En az 2 kelime olmalı
                throw new ArgumentException("Invalid full name format. Expected 'FirstName LastName'.");

            string firstName = parts[0];
            string lastName = string.Join(" ", parts.Skip(1)); // Geri kalanları soyad olarak al

            return new FullName(firstName, lastName);
        }

        public static implicit operator string(FullName fullName) => fullName.ToString();
        public static implicit operator FullName(string value) => Create(value);
        public override string ToString() => $"{FirstName} {LastName}";
        public string GetInitials() => $"{FirstName[0]}.{LastName[0]}";
    }