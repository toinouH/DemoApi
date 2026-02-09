using DemoApi.Models;

namespace DemoApi.Services;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task<Supplier> CreateAsync(string name);
    Task<bool> UpdateNameAsync(int id, string newName);

    Task<(bool ok, string? error)> DeleteAsync(int id);
}