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
    public DbSet<ProductRawMaterials> ProductRawMaterials { get; set; }

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
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductRawMaterials>()
            .HasKey(x => new {x.ProductId, x.RawMaterialId});

        modelBuilder.Entity<ProductRawMaterials>()
            .HasOne(x => x.RawMaterial)
            .WithMany(r => r.ProductRawMaterials)
            .HasForeignKey(x => x.RawMaterialId);

        modelBuilder.Entity<ProductRawMaterials>()
            .HasOne(x => x.Product)
            .WithMany(p => p.ProductRawMaterials)
            .HasForeignKey(x => x.ProductId);


    }
}