namespace MilkTeaShop.Models;

public class AdminDashboardViewModel
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int PendingOrders { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
    public List<Product> TopProducts { get; set; } = new();
}
