using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Restaurant.Cli.Models;

namespace Restaurant.Cli.Data;

public class AppDbContext : DbContext
{
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("Default") ?? "Data Source=app.db";
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>()
            .HasIndex(x => x.Number)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasOne(x => x.Table)
            .WithMany(x => x.Orders!)
            .HasForeignKey(x => x.TableId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.MenuItem)
            .WithMany(x => x.OrderItems!)
            .HasForeignKey(x => x.MenuItemId);
    }
}