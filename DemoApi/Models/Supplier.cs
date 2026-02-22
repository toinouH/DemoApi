using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoApi.Models;

public class Supplier
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Le nom du fournisseur")]
    [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}