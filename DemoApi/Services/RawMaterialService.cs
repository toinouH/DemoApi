using DemoApi.Data;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Services;

public class RawMaterialService : IRawMaterialService
{
    private readonly AppDbContext _context;

    public RawMaterialService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<RawMaterial>> GetAllAsync()
    {
        return await _context.RawMaterials
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RawMaterial?> GetByIdAsync(int id)
    {
        return await _context.RawMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<RawMaterial> CreateAsync(string name)
    {

        var rawMaterial = new RawMaterial
        {
            Name = name
        };

        _context.RawMaterials.Add(rawMaterial);
        await _context.SaveChangesAsync();
        return rawMaterial;
    }


    public async Task<IEnumerable<RawMaterial>> GetAllByProductAsync(int productId)
    {
        Product? product = await _context.Products.FindAsync(productId);

        if (product == null)
            throw new Exception("Supplier introuvable");

        return await _context.RawMaterials
        .AsNoTracking()
        .Where(r => r.Id == productId)
        .Include(r => r.ProductRawMaterials)
            .ThenInclude(prm=>prm.Product)
        .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rawMaterial = await _context.RawMaterials.FindAsync(id);

        if (rawMaterial == null)
            return false;

        _context.RawMaterials.Remove(rawMaterial);
        await _context.SaveChangesAsync();

        return true;
    }
}
