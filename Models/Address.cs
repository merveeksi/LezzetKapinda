using LezzetKapinda.DomainEvents;
using LezzetKapinda.Entities;
using LezzetKapinda.ValueObjects;
using TSID.Creator.NET;

namespace LezzetKapinda.Models;

public sealed class Address : EntityBase<long>
{
    public long UserId { get; set; }
    public Address? FullAddress { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }
    public bool IsDefault { get; set; }

    // Relationships
    public User? User { get; set; }
    
    
    public Address(long id, long userId, Address fullAddress, string city, string postalCode, PhoneNumber phoneNumber,
        bool isDefault, User user)
    {
        Id = id;
        UserId = userId;
        FullAddress = fullAddress;
        City = city;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        IsDefault = isDefault;
        User = user;
    }

    public Address()
    {

    }
    
    public static Address Create(long userId, Address fullAddress, string city, string postalCode, PhoneNumber phoneNumber,
        bool isDefault, User user)
    {
        var address = new Address
        {
            Id = TsidCreator.GetTsid().ToLong(),
            UserId = userId,
            FullAddress = fullAddress,
            City = city,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
            IsDefault = isDefault,
            User = user
        };
        address.RaiseDomainEvent(new AddressCreatedDomainEvent(address.Id));
        return address;
    }
}