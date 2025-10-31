using Microsoft.EntityFrameworkCore;
using Restaurant.Cli.Data;
using Restaurant.Cli.Models;

using var db = new AppDbContext();
await db.Database.MigrateAsync();

await SeedDataAsync(db);

Console.WriteLine("=== Restaurant ===");
Console.WriteLine("[1] List Tables");
Console.WriteLine("[2] List Menu Item");
Console.WriteLine("[0] Exit");
Console.WriteLine("Choose: ");
var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        ListTables(db);
        break;
    case "2":
        ListMenuItems(db);
        break;
    default:
        Console.WriteLine("Default Statement");
        break;
}


static async Task SeedDataAsync(AppDbContext db)
{
    var changed = false;
    if (!await db.Tables.AnyAsync())
    {
        db.Tables.AddRange(
            new Table { Number = 1, Seats = 2, Status = "Available" },
            new Table { Number = 2, Seats = 4, Status = "Available" }
        );
        changed = true;
    }

    if (!await db.MenuItems.AnyAsync())
    {
        db.MenuItems.AddRange(
            new MenuItem { Name = "Coca Cola", Category = "Drinks", Price = 1.49m, IsAvailable = true },
            new MenuItem { Name = "Chocolate Cake", Category = "Dessert", Price = 7.99m, IsAvailable = true }
        );
        changed = true;
    }

    if (changed)
    {
        await db.SaveChangesAsync();
    }
}

static void ListTables(AppDbContext db)
{
    Console.WriteLine("\n-- Tables --");
    foreach (var x in db.Tables.AsNoTracking().OrderBy(x => x.Number))
    {
        Console.WriteLine($"Table #{x.Number} | Seats: {x.Seats} | Status: {x.Status}");
    }
}

static void ListMenuItems(AppDbContext db)
{
    Console.WriteLine("\n-- Menu Items --");
    foreach (var x in db.MenuItems.AsNoTracking().OrderBy(x => x.Category).ThenBy(x => x.Name))
    {
        var availability = x.IsAvailable ? "" : "[UNAVAILABLE]";
        Console.WriteLine($"{x.Category} | {x.Name} | ${x.Price} {availability}");
    }
}