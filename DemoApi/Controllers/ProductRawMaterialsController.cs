using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductRawMaterialsController : ControllerBase
{
    private readonly IProductRawMaterialService _service;

    public ProductRawMaterialsController(IProductRawMaterialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, [FromBody] AddRawMaterialToProductRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (ok, error) = await _service.AddAsync(productId, request.RawMaterialId, request.Quantity);

        if (ok) return NoContent();

        // erreurs "métier"
        if (error == "Produit introuvable" || error == "Matière première introuvable")
            return NotFound(new { message = error });

        return Conflict(new { message = error });
    }

    [HttpDelete("{rawMaterialId:int}")]
    public async Task<IActionResult> Delete(int productId, int rawMaterialId)
    {
        var (ok, error) = await _service.DeleteAsync(productId, rawMaterialId);

        if (ok) return NoContent();

        return NotFound(new { message = error });
    }

    public class AddRawMaterialToProductRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "RawMaterialId invalide")]
        public int RawMaterialId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity doit être > 0")]
        public int Quantity { get; set; }
    }
}
