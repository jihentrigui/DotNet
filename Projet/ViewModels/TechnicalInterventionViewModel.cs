using System.ComponentModel.DataAnnotations;

namespace Projet.ViewModels
{
    public class TechnicalInterventionViewModel
    {
        public int TechnicalInterventionId { get; set; }

        [Required(ErrorMessage = "La réclamation est requise.")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "La date de l'intervention est requise.")]
        public DateTime InterventionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le statut de garantie est requis.")]
        public bool IsWarranty { get; set; }

        [Required(ErrorMessage = "Le coût de la main-d'œuvre est requis.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le coût de la main-d'œuvre doit être positif.")]
        public decimal LaborCost { get; set; }

        public decimal TotalCost { get; set; }

        // Liste des pièces utilisées (ID des pièces)
        public List<int> SparePartIds { get; set; } = new List<int>();
    }
}
