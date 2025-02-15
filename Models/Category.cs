namespace LezzetKapinda.Models;

public sealed record Category
{
    public long Id { get; set; }
    public string CategoryName { get; set; }
    

    // Relationships
    public ICollection<Product> Products { get; set; }
}