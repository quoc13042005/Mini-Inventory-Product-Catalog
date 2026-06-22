using Microsoft.EntityFrameworkCore;
using AspNetWeek2.Mvc.Models;

namespace AspNetWeek2.Mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
                
            entity.HasIndex(p => p.SKU).IsUnique();
            entity.Property(p => p.RowVersion).IsRowVersion();
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Accessories" },
            new Category { Id = 2, Name = "Displays" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Wireless Mouse", SKU = "WM-001", Price = 250000, StockQuantity = 10, CategoryId = 1, CreatedAt = new DateTime(2023, 1, 1) },
            new Product { Id = 2, Name = "Mechanical Keyboard", SKU = "MK-002", Price = 1350000, StockQuantity = 4, CategoryId = 1, CreatedAt = new DateTime(2023, 1, 1) },
            new Product { Id = 3, Name = "24-Inch Monitor", SKU = "MON-024", Price = 3200000, StockQuantity = 3, CategoryId = 2, CreatedAt = new DateTime(2023, 1, 1) }
        );
    }
}
