using System.ComponentModel.DataAnnotations;

namespace Projet.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // e.g., "Sanitary", "Heating", "Spare Part"

        [Required]
        public decimal Price { get; set; } // Price of the article

        public bool IsUnderWarranty { get; set; } = false; // Whether the article is under warranty

        public int StockQuantity { get; set; } = 0; // Available stock quantity


        public string Description { get; set; } = string.Empty; // Description of the article
    }
}