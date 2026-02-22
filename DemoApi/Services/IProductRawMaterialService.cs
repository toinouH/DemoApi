using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IProductRawMaterialService
    {

        Task<(bool ok, string? error)> AddAsync(int productId, int rawMaterialId, int quantity);
        Task<(bool ok, string? error)> DeleteAsync(int productId, int rawMaterialId);

    }
}