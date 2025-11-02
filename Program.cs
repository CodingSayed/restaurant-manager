using Microsoft.EntityFrameworkCore;
using Restaurant.Cli.Data;
using Restaurant.Cli.Models;
using Restaurant.Cli.Services;
using Restaurant.Cli.Menus;

using var db = new AppDbContext();
db.Database.Migrate();
SeedData(db);

var tableService = new TableService(db);
var tablesMenu = new TablesMenu(tableService);
var menuItemService = new MenuItemService(db);
var menuItemMenu = new MenuItemsMenu(menuItemService);
var orderService = new OrderService(db);
var ordersMenu = new OrdersMenu(orderService, menuItemService);

while (true)
{
    Console.Clear();
    ConsoleHelpers.PrintHeader("Restaurant Manager");
    Console.WriteLine("1) Manage Tables");
    Console.WriteLine("2) Manage Menu Items");
    Console.WriteLine("3) Manage Orders");
    Console.WriteLine("0) Exit");
    Console.Write("\nSelect a number: ");
    var choice = (Console.ReadLine() ?? "").Trim();

    if (choice == "0") break;
    if (choice == "1") tablesMenu.ShowMenu();
    if (choice == "2") menuItemMenu.ShowMenu();
    if (choice == "3") ordersMenu.ShowMenu();
    else Console.WriteLine("Invalid option");
}

Console.WriteLine("Au revoir!");




static void SeedData(AppDbContext db)
{
    var changed = false;

    if (!db.Tables.Any())
    {
        db.Tables.AddRange(
            new Table { Number = 1, Seats = 2, Status = "Available" },
            new Table { Number = 2, Seats = 4, Status = "Available" }
        );
        changed = true;
    }

    if (!db.MenuItems.Any())
    {
        db.MenuItems.AddRange(
            new MenuItem { Name = "Margherita Pizza", Category = "Pizza", Price = 8.50m, IsAvailable = true },
            new MenuItem { Name = "Espresso", Category = "Drinks", Price = 2.20m, IsAvailable = true }
        );
        changed = true;
    }

    if (changed) db.SaveChanges();
}