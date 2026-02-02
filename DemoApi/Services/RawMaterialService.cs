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

        _context.Products.Add(rawMaterial);
        await _context.SaveChangesAsync();
        return rawMaterial;
}}