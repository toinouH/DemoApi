using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static DemoApi.Controllers.ProductsController;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]



public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateSupplierRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var supplier = await _supplierService.CreateAsync(request.Name);
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id:int}/name")]
    public async Task<IActionResult> UpdateName(int id, [FromQuery] string newName)
    {
        if (newName==null)
        {
            return BadRequest("Le nom ne peux pas être null");
        }

        var success = await _supplierService.UpdateNameAsync(id, newName);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (ok, error) = await _supplierService.DeleteAsync(id);

        if (ok) return NoContent();

        if (error == "Supplier introuvable")
            return NotFound(new { message = error });

        return Conflict(new { message = error }); // 409
    }


    public class CreateSupplierRequest
    {
        [Required(ErrorMessage = "Le nom du fournisseur est requis")]
        [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
        public string Name { get; set; } = string.Empty;

    }
}
