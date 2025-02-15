using LezzetKapinda.DomainEvents;
using LezzetKapinda.Entities;
using LezzetKapinda.ValueObjects;
using TSID.Creator.NET;

namespace LezzetKapinda.Models;

public sealed class User : EntityBase<long>
{
    public FullName FullName { get; set; }
    public UserName UserName { get; set; }
    public string? Comment { get; set; }
    public string ImageUrl { get; set; }
    public Email Email { get; set; }
    
    private Password _password; // ✅ EF Core'un saklaması için backing field kullanacağız.
    public Password PasswordHash
    {
        get => _password;
        private set => _password = value;
    }
    public string Role { get; set; } // "Customer" veya "Saller"
    public int Rating { get; set; }
    
    // Relationships
    public ICollection<Address> Addresses { get; set; }
    public ICollection<Order> Orders { get; set; }
    
    
    public User(Password password, FullName fullName, UserName userName, string comment, string imageUrl, Email email, string role, int rating)
    {
        _password = password;
        FullName = fullName;
        UserName = userName;
        Comment = comment;
        ImageUrl = imageUrl;
        Email = email;
        Role = role;
        Rating = rating;
    }

    public User()
    {
        
    }
    public static User Create(Password password, FullName fullName, UserName userName, string comment, string imageUrl, Email email, string role, int rating)
    {
        var user = new User
        {
            Id = TsidCreator.GetTsid().ToLong(),
            PasswordHash = password,
            FullName = fullName,
            UserName = userName,
            Comment = comment,
            ImageUrl = imageUrl,
            Email = email,
            Role = role,
            Rating = rating
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
    
    // Şifreyi hashleme ve set etme
    public void SetPassword(string password)
    {
        PasswordHash = new Password(password); // Password value object olarak kullanılıyor
    }

    // Şifre doğrulama fonksiyonu
    public bool VerifyPassword(string password)
    {
        return PasswordHash == new Password(password);
    }
}