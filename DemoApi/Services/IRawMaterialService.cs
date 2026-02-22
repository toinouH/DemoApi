using DemoApi.Models;

namespace DemoApi.Services;

public interface IRawMaterialService
{
    Task<IEnumerable<RawMaterial>> GetAllAsync();
    Task<RawMaterial?> GetByIdAsync(int id);
    Task<RawMaterial> CreateAsync(string name);
    Task<IEnumerable<RawMaterial>> GetAllByProductAsync(int productId);
    Task<bool> DeleteAsync(int id);
}