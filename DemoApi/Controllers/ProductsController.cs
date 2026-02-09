using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DemoApi.Services;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("expensive")]
    public async Task<ActionResult<IEnumerable<object>>> GetExpensive([FromQuery] decimal minPrice)
    {
        var products = await _productService.GetMoreExpensiveThanAsync(minPrice);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _productService.CreateAsync(request.Name, request.Price,request.SupplierId);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}/price")]
    public async Task<IActionResult> UpdatePrice(int id, [FromQuery] decimal newPrice)
    {
        if (newPrice < 0.01m || newPrice > 1000000m)
        {
            return BadRequest("Le prix doit être entre 0.01 et 1,000,000");
        }

        var success = await _productService.UpdatePriceAsync(id, newPrice);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpGet("{SupplierId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetAllBySupplier(int SupplierId)
    {
        var Supplierproducts = await _productService.GetAllBySupplierAsync(SupplierId);
        return Ok(Supplierproducts);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Le nom du produit est requis")]
        [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Le prix doit être entre 0.01 et 1,000,000")]
        public decimal Price { get; set; }

        public int SupplierId { get; set; }
    }
}