using Restaurant.Cli.Services;

namespace Restaurant.Cli.Menus;

public class MenuItemsMenu
{
    private readonly MenuItemService _menuItemService;

    public MenuItemsMenu(MenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    public void ShowMenu()
    {
        while (true)
        {   
            Console.Clear();
            Console.WriteLine("=== Menu Items ===");
            Console.WriteLine("1) List");
            Console.WriteLine("2) Create");
            Console.WriteLine("3) Edit");
            Console.WriteLine("4) Delete");
            Console.WriteLine("0) Back");
            Console.WriteLine("\n Select a number: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            try
            {
                switch (choice)
                {
                    case "1": List(); break;
                    case "2": Create(); break;
                    case "3": Edit(); break;
                    case "4": Delete(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }

    }

    private void List()
    {
        var items = _menuItemService.GetAll();
        if (items.Count == 0) { Console.WriteLine("No menu items found"); return; }

        Console.WriteLine($"\n{"#",-4}| {"Category", -15}| {"Name", -20}| {"Price", -6}| Available");
        Console.WriteLine("------------------------------------------------------------------------");

        foreach (var item in items)
        {
            Console.WriteLine($"{item.Id,-4}| {item.Category,-15}| {item.Name,-20}| ${item.Price,-6}| {(item.IsAvailable ? "Yes" : "No")} ");
        }

    }

    private void Create()
    {
        var name = ConsoleHelpers.Prompt("Name");
        var category = ConsoleHelpers.Prompt("Category");
        var price = DecimalValidation("Price");
        _menuItemService.Create(name, category, price);
        Console.WriteLine("Menu item created");
    }


    private void Edit()
    {
        var id = ConsoleHelpers.PromptInt("Id");
        var name = ConsoleHelpers.Prompt("New name");
        var category = ConsoleHelpers.Prompt("New category");

        decimal? price = null;
        var priceRaw = ConsoleHelpers.Prompt("New price");
        if (!string.IsNullOrWhiteSpace(priceRaw))
        {
            if (!decimal.TryParse(priceRaw, out var p) || price <= 0)
            {
                Console.WriteLine("Invalid Price");
                return;
            }
            price = p;
        }

        bool? available = null;
        var availableRaw = ConsoleHelpers.Prompt("Available? [y/n]");
        if (!string.IsNullOrWhiteSpace(availableRaw))
        {
            var s = availableRaw.Trim().ToLowerInvariant();
            if (s is "y" or "yes") available = true;
            else if (s is "n" or "no") available = false;
            else { Console.WriteLine("Invalid availability (y or yes / n or no)"); return; }
        }

        _menuItemService.Update(
            id,
            string.IsNullOrWhiteSpace(name) ? "" : name,
            string.IsNullOrWhiteSpace(category) ? "" : category,
            price,
            available);

        Console.WriteLine("Menu item updated");
    }
    
    private void Delete()
    {
      
        var id = ConsoleHelpers.PromptInt("ID", min: 1);
        if (!ConsoleHelpers.Confirm($"Delete menu item #{id}?")) return;
        _menuItemService.Delete(id);
        Console.WriteLine($"Menu item with id '{id}' deleted");
    
    }



    private static decimal DecimalValidation(string label)
    {
        while (true)
        {
            var raw = ConsoleHelpers.Prompt(label);
            if (decimal.TryParse(raw, out var value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Please enter a valid price (price > 0 and two decimals)");
        }
    }



}