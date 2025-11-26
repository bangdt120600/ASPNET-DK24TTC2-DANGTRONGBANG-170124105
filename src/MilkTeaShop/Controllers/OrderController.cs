using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Data;
using MilkTeaShop.Models;
using System.Text.Json;

namespace MilkTeaShop.Controllers;

public class OrderController : Controller
{
    private readonly AppDbContext _context;
    private const string CartSessionKey = "Cart";

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (!cart.Any()) return RedirectToAction("Index", "Cart");
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string customerName, string phone, string address, string note, string paymentMethod)
    {
        var cart = GetCart();
        if (!cart.Any()) return RedirectToAction("Index", "Cart");

        var order = new Order
        {
            CustomerName = customerName,
            Phone = phone,
            Address = address,
            Note = note,
            PaymentMethod = paymentMethod,
            TotalAmount = cart.Sum(c => c.Total),
            OrderDate = DateTime.Now,
            Status = "Pending"
        };

        foreach (var item in cart)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity,
                Size = item.Size,
                Sugar = item.Sugar,
                Ice = item.Ice,
                Topping = item.Topping
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        HttpContext.Session.Remove(CartSessionKey);
        return RedirectToAction("Confirmation", new { id = order.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        return View(order);
    }

    private List<CartItem> GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }
}
