using System.ComponentModel.DataAnnotations;
using LezzetKapinda.Models;

namespace LezzetKapinda.ViewModels;

public sealed class OrderViewModel
{
    public long AddressId { get; set; }
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<User> Users { get; set; } = new List<User>();
    public IEnumerable<Address> Addresses { get; set; } = new List<Address>();
    public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
    public Address Address { get; set; }

    public OrderViewModel(IEnumerable<Address> addresses, IEnumerable<Order> orders, IEnumerable<OrderItem> orderItems, IEnumerable<Product> products, IEnumerable<User> users, IEnumerable<Payment> payments, Address address)
    {
        Orders = orders;
        OrderItems = orderItems;
        Products = products;
        Users = users;
        Payments = payments;
        Address = address;
    }

    public OrderViewModel()
    {
    }
}