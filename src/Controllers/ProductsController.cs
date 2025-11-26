using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaShop.Data;

namespace MilkTeaShop.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        var products = categoryId.HasValue
            ? await _context.Products.Where(p => p.CategoryId == categoryId && p.IsActive).Include(p => p.Category).ToListAsync()
            : await _context.Products.Where(p => p.IsActive).Include(p => p.Category).ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }
}
