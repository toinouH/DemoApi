using Microsoft.EntityFrameworkCore;
using DemoApi.Data;
using DemoApi.Models;

namespace DemoApi.Services;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;

    public SupplierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier> CreateAsync(string name)
    {
        var supplier = new Supplier
        {
            Name = name
        };
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }
    public async Task<bool> UpdateNameAsync(int id, string newName)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return false;
        }

        supplier.Name = newName;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    
    public async Task<(bool ok, string? error)> DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return (false, "Supplier introuvable");

        bool hasProducts = await _context.Products
            .AnyAsync(p => p.SupplierId == id);

        if (hasProducts)
            return (false, "Impossible de supprimer : des produits sont encore liés à ce supplier.");

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}