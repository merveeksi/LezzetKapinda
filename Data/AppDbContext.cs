using LezzetKapinda.Configurations;
using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using UserOrderImages = LezzetKapinda.Models.UserOrderImages;

namespace LezzetKapinda.Data;

public sealed class AppDbContext : DbContext
{
    private readonly DbContextOptions<AppDbContext> _options;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _options = options;
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<Counter> Counters { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOrderImages> UserOrderImages { get; set; }
}