using LezzetKapinda.Models;
using LezzetKapinda.ValueObjects;

namespace LezzetKapinda.ViewModels;

public sealed class HomeViewModel
{
    public FullName FullName { get; set; }
    public IEnumerable<User> Users { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<Testimonial> Testimonials { get; set; }
    public IEnumerable<Counter> Counters { get; set; }
    public IEnumerable<UserOrderImages> UserOrderImages { get; set; }
}