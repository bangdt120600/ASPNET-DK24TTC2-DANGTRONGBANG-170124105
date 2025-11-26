using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Models;

namespace MilkTeaShop.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Promotion> Promotions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Trà Sữa Truyền Thống" },
            new Category { Id = 2, Name = "Trà Sữa Trái Cây" },
            new Category { Id = 3, Name = "Trà Sữa Đặc Biệt" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Trà Sữa Trân Châu", Description = "Trà sữa truyền thống với trân châu đen", Price = 35000, CategoryId = 1, ImageUrl = "/images/trasua1.jpg" },
            new Product { Id = 2, Name = "Trà Sữa Matcha", Description = "Trà sữa matcha Nhật Bản", Price = 40000, CategoryId = 1, ImageUrl = "/images/trasua2.jpg" },
            new Product { Id = 3, Name = "Trà Sữa Dâu", Description = "Trà sữa vị dâu tươi", Price = 38000, CategoryId = 2, ImageUrl = "/images/trasua3.jpg" },
            new Product { Id = 4, Name = "Trà Sữa Đào", Description = "Trà sữa đào thơm ngon", Price = 38000, CategoryId = 2, ImageUrl = "/images/trasua4.jpg" },
            new Product { Id = 5, Name = "Trà Sữa Socola", Description = "Trà sữa socola đậm đà", Price = 42000, CategoryId = 3, ImageUrl = "/images/trasua5.jpg" },
            new Product { Id = 6, Name = "Trà Sữa Kem Cheese", Description = "Trà sữa phủ kem cheese", Price = 45000, CategoryId = 3, ImageUrl = "/images/trasua6.jpg" }
        );
    }
}
