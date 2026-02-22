using DemoApi.Models;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly IRawMaterialService _rawMaterialService;

    public RawMaterialsController(IRawMaterialService rawMaterialService)
    {
        _rawMaterialService = rawMaterialService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var rawMaterials = await _rawMaterialService.GetAllAsync();
        return Ok(rawMaterials);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var rawMaterial = await _rawMaterialService.GetByIdAsync(id);
        if (rawMaterial == null)
        {
            return NotFound();
        }
        return Ok(rawMaterial);
    }

    [HttpGet("/ProductRawMaterials/{id:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetAllByProduct(int id)
    {
        var ProductRawMaterials = await _rawMaterialService.GetAllByProductAsync(id);
        return Ok(ProductRawMaterials);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _rawMaterialService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreaterawMaterialRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rawMaterial = await _rawMaterialService.CreateAsync(request.Name);
        return CreatedAtAction(nameof(GetById), new { id = rawMaterial.Id }, rawMaterial);
    }



    public class CreaterawMaterialRequest
    {
        [Required(ErrorMessage = "Le nom de la matiere premiere est requis")]
        [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
        public string Name { get; set; } = string.Empty;
       
    }
}