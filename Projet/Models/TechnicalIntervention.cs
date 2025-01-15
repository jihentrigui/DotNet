
using Projet.Models;

namespace Projet.Models
{
    public class TechnicalIntervention
    {
        public int TechnicalInterventionId { get; set; }
        public int ClaimId { get; set; }
        public DateTime InterventionDate { get; set; } = DateTime.Now;

        // Indiquer si l'intervention est sous garantie ou non
        public bool IsWarranty { get; set; }

        // Coût de la main-d'œuvre
        public decimal LaborCost { get; set; }

        // Coût total calculé
        public decimal TotalCost { get; set; }

        // Pièces utilisées dans l'intervention
        public ICollection<SparePart> SparePartsUsed { get; set; } = new List<SparePart>();

        public Claims Claim { get; set; } = null!;

        // Méthode pour calculer le coût total
        public void CalculateTotalCost()
        {
            if (IsWarranty)
            {
                // Si l'article est sous garantie, l'intervention est gratuite
                TotalCost = 0.0m;
            }
            else
            {
                // Si l'intervention est hors garantie, calculer le coût total
                decimal sparePartsCost = SparePartsUsed.Sum(part => part.Price); // Coût total des pièces utilisées
                TotalCost = sparePartsCost + LaborCost; // Ajouter le coût des pièces et de la main-d'œuvre
            }
        }
    }
}
