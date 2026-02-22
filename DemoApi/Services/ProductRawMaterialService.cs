using Microsoft.EntityFrameworkCore;
using DemoApi.Data;
using DemoApi.Models;

namespace DemoApi.Services;

public class ProductRawMaterialService : IProductRawMaterialService
{
    private readonly AppDbContext _context;

    public ProductRawMaterialService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool ok, string? error)> AddAsync(int productId, int rawMaterialId, int quantity)
    {
        if (quantity <= 0)
            return (false, "La quantité doit être > 0");

        // vérifier existence
        if (!await _context.Products.AnyAsync(p => p.Id == productId))
            return (false, "Produit introuvable");

        if (!await _context.RawMaterials.AnyAsync(r => r.Id == rawMaterialId))
            return (false, "Matière première introuvable");

        // vérifier doublon
        var existing = await _context.ProductRawMaterials
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.RawMaterialId == rawMaterialId);

        if (existing != null)
            return (false, "Relation déjà existante");

        _context.ProductRawMaterials.Add(new ProductRawMaterials
        {
            ProductId = productId,
            RawMaterialId = rawMaterialId,
            Quantity = quantity
        });

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool ok, string? error)> DeleteAsync(int productId, int rawMaterialId)
    {
        var link = await _context.ProductRawMaterials
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.RawMaterialId == rawMaterialId);

        if (link == null)
            return (false, "Relation introuvable");

        _context.ProductRawMaterials.Remove(link);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}
