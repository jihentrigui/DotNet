namespace Projet.Models
{
    public class SparePart
    {
        public int SparePartId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
