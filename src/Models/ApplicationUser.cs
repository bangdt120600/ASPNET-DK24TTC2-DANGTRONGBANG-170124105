using Microsoft.AspNetCore.Identity;

namespace MilkTeaShop.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Points { get; set; } = 0;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
