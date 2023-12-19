using Microsoft.EntityFrameworkCore;

namespace Pedrydev.Messaging.Inventory.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
