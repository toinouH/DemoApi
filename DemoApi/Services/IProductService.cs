using DemoApi.Models;

namespace DemoApi.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetMoreExpensiveThanAsync(decimal minPrice);
    Task<Product> CreateAsync(string name, decimal price);
    Task<bool> UpdatePriceAsync(int id, decimal newPrice);
}