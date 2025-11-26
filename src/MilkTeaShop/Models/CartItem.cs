namespace MilkTeaShop.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; } = "M";
    public int Sugar { get; set; } = 100;
    public int Ice { get; set; } = 100;
    public string Topping { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Total => Price * Quantity;
}
