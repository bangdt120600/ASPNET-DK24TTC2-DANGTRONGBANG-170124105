using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Data;
using MilkTeaShop.Models;

namespace MilkTeaShop.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalOrders = await _context.Orders.CountAsync(),
            TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount),
            TotalProducts = await _context.Products.CountAsync(),
            TotalCustomers = await _userManager.Users.CountAsync(),
            PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
            RecentOrders = await _context.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToListAsync(),
            TopProducts = await _context.Products.Where(p => p.IsActive).Take(5).ToListAsync()
        };
        return View(model);
    }

    public async Task<IActionResult> Products()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Orders()
    {
        var orders = await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Orders");
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Products");
    }

    public async Task<IActionResult> Customers()
    {
        var customers = await _userManager.Users.ToListAsync();
        return View(customers);
    }

    public async Task<IActionResult> Promotions()
    {
        var promotions = await _context.Promotions.ToListAsync();
        return View(promotions);
    }

    [HttpGet]
    public IActionResult CreatePromotion() => View();

    [HttpPost]
    public async Task<IActionResult> CreatePromotion(Promotion promotion)
    {
        if (ModelState.IsValid)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm khuyến mãi thành công!";
            return RedirectToAction("Promotions");
        }
        return View(promotion);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePromotion(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion != null)
        {
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa khuyến mãi thành công!";
        }
        return RedirectToAction("Promotions");
    }

    public async Task<IActionResult> Categories()
    {
        var categories = await _context.Categories.Include(c => c.Products).ToListAsync();
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _context.Categories.Add(new Category { Name = name });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm danh mục thành công!";
        }
        return RedirectToAction("Categories");
    }
}
