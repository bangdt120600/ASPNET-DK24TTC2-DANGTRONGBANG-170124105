using Microsoft.AspNetCore.Mvc;
using MilkTeaShop.Models;
using System.Text.Json;

namespace MilkTeaShop.Controllers;

public class CartController : Controller
{
    private const string CartSessionKey = "Cart";

    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, string productName, decimal price, string imageUrl, 
        int quantity = 1, string size = "M", int sugar = 100, int ice = 100, string topping = "")
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size && c.Sugar == sugar && c.Ice == ice && c.Topping == topping);
        
        if (item != null)
            item.Quantity += quantity;
        else
            cart.Add(new CartItem { ProductId = productId, ProductName = productName, Price = price, ImageUrl = imageUrl, Quantity = quantity, Size = size, Sugar = sugar, Ice = ice, Topping = topping });

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int productId, string size, int sugar, int ice, string topping, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size && c.Sugar == sugar && c.Ice == ice && c.Topping == topping);
        if (item != null)
        {
            if (quantity > 0) item.Quantity = quantity;
            else cart.Remove(item);
            SaveCart(cart);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int productId, string size, int sugar, int ice, string topping)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size && c.Sugar == sugar && c.Ice == ice && c.Topping == topping);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Clear()
    {
        HttpContext.Session.Remove(CartSessionKey);
        return RedirectToAction("Index");
    }

    private List<CartItem> GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }
}
