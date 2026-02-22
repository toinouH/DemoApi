using Microsoft.EntityFrameworkCore;
using DemoApi.Data;
using DemoApi.Models;

namespace DemoApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Supplier)
            .Include(p => p.ProductRawMaterials)
                .ThenInclude(prm => prm.RawMaterial)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Supplier)
            .Include(p => p.ProductRawMaterials)
                .ThenInclude(prm => prm.RawMaterial)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetMoreExpensiveThanAsync(decimal minPrice)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Price > minPrice)
            .OrderBy(p => p.Price)
            .Include(p => p.Supplier)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(string name, decimal price, int supplierId)
    {
        Supplier? supplier = await _context.Suppliers.FindAsync(supplierId);

        if (supplier == null)
            throw new Exception("Supplier introuvable");

        var product = new Product
        {
            Name = name,
            Price = price,
            SupplierId = supplierId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;

    }



    

    public async Task<bool> UpdatePriceAsync(int id, decimal newPrice)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Price = newPrice;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Product>> GetAllBySupplierAsync(int supplierId)
    {
        return await _context.Products
        .AsNoTracking()
        .Where(p => p.SupplierId == supplierId)
        .Include(p => p.Supplier)
        .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<RawMaterial>> GetAllRawMaterialsByProductAsync(int productId)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
        if (!productExists)
            throw new Exception("Produit introuvable");

        return await _context.ProductRawMaterials
            .AsNoTracking()
            .Where(prm => prm.ProductId == productId)
            .Include(prm => prm.RawMaterial)
            .Select(prm => prm.RawMaterial)
            .ToListAsync();
    }
}