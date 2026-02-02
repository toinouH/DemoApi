using DemoApi.Data;

namespace DemoApi.Services;

public class RawMaterialService : IRawMaterialService
{
    private readonly AppDbContext _context;

    public RawMaterialService(AppDbContext context)
    {
        _context = context;
    }
}