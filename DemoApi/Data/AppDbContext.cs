using Microsoft.EntityFrameworkCore;
using DemoApi.Models;

namespace DemoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<RawMaterial> RawMaterials { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
       .HasOne(p => p.Supplier)
       .WithMany(s => s.Products)
       .HasForeignKey(p => p.SupplierId)
       .OnDelete(DeleteBehavior.Restrict);

    }
}