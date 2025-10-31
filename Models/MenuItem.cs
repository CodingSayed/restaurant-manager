namespace Restaurant.Cli.Models;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<OrderItem>? OrderItems { get; set; }
}