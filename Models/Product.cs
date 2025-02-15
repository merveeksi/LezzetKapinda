using System.ComponentModel.DataAnnotations;

namespace LezzetKapinda.Models;

public sealed class Product
{
    public Product(int id, string name, string description, decimal price, string imageUrl, bool isActive, int categoryId, Category category)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        IsActive = isActive;
        CategoryId = categoryId;
        Category = category;
    }

    public Product()
    {
            
    }

    public long Id { get; set; }
        
    public long CategoryId { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
       
        
    // Relationships
    public ICollection<OrderItem> OrderItems { get; set; }
    public Category Category { get; set; }
        
}
