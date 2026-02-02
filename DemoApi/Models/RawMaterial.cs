using System.ComponentModel.DataAnnotations;

namespace DemoApi.Models;

public class RawMaterial
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Le nom du produit est requis")]
    [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
    public string Name { get; set; } = string.Empty;
}