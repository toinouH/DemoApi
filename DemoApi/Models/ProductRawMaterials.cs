namespace DemoApi.Models
{
    public class ProductRawMaterials
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public int Quantity { get; set; }
    }
}
