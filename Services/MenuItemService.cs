using Microsoft.EntityFrameworkCore;
using Restaurant.Cli.Data;
using Restaurant.Cli.Models;

namespace Restaurant.Cli.Services;

public class MenuItemService
{
    private readonly AppDbContext _db;
    public MenuItemService(AppDbContext db) => _db = db;

    public List<MenuItem> GetAll() =>
        _db.MenuItems.AsNoTracking().OrderBy(x => x.Category).ThenBy(x => x.Name).ToList();

    public void Create(string name, string category, decimal price)
    {
        _db.MenuItems.Add(new MenuItem { Name = name, Category = category, Price = price, IsAvailable = true });
        _db.SaveChanges();
    }

    public void Update(int id, string? name, string? category, decimal? price, bool? available)
    {
        var item = _db.MenuItems.FirstOrDefault(x => x.Id == id)
                   ?? throw new Exception("Menu item not found.");

        if (!string.IsNullOrWhiteSpace(name)) item.Name = name;
        if (!string.IsNullOrWhiteSpace(category)) item.Category = category;
        if (price is not null) item.Price = price.Value;
        if (available is not null) item.IsAvailable = available.Value;
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = _db.MenuItems.FirstOrDefault(x => x.Id == id)
                   ?? throw new Exception("Menu item not found.");
        _db.MenuItems.Remove(item);
        _db.SaveChanges();
    }
}