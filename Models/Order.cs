using LezzetKapinda.Enums;

namespace LezzetKapinda.Models;

public sealed record Order
{
    public Order(long id, long userId, string userName, Address address, DateTimeOffset orderDate, Status status, decimal totalAmount, ICollection<OrderItem> orderItems, User user)
    {
        Id = id;
        UserId = userId;
        UserName = userName;
        Address = address;
        OrderDate = orderDate;
        Status = status;
        TotalAmount = totalAmount;
        OrderItems = orderItems;
        User = user;
    }
    
    public Order()
    {
        
    }

    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public Address Address { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public Status Status { get; set; } // Sipariş durumu
    public decimal TotalAmount { get; set; } // Toplam tutar

    // Relationships
    public ICollection<OrderItem> OrderItems { get; set; }
    public User User { get; set; }
}
