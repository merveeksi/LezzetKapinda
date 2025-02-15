namespace LezzetKapinda.Models;

public sealed record OrderItem
{
    public OrderItem(long id, long orderId, long productId, string productName, int quantity, decimal price, decimal total, Order order, Product product)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        Total = total;
        Order = order;
        Product = product;
    }
    
    public OrderItem()
    {
        
    }

    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; } // Kaç adet yemek siparişi verildi
    public decimal Price { get; set; } //fiyat
    public decimal Total { get; set; } // Yemek fiyatı * Miktar

    // Relationships
    public Order Order { get; set; }
    public Product Product { get; set; }
}