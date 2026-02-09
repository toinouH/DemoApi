using System.ComponentModel.DataAnnotations;

namespace DemoApi.Models;

public class Product {
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom du produit est requis")]
    [MinLength(1, ErrorMessage = "Le nom doit contenir au moins 1 caractère")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "Le prix doit être entre 0.01 et 1,000,000")]
    public decimal Price { get; set; }
    // Clé étrangère
    public int SupplierId { get; set; }

    // Navigation vers Supplier
    public Supplier Supplier { get; set; }
}