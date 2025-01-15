using System.ComponentModel.DataAnnotations;

namespace Projet.ViewModels
{
    public class ClaimViewModel
    {
        [Required(ErrorMessage = "L'article est requis.")]
        public int ArticleId { get; set; }

       

        [Required(ErrorMessage = "La description est requise.")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le statut est requis.")]
        public string Status { get; set; }
    }
}
