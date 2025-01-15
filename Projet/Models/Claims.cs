using Microsoft.AspNetCore.Identity;
using Projet.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Models
{
    public class Claims
    {
        [Key]
        public int ClaimId { get; set; }


        // Lien direct avec IdentityUser
        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }



        [ForeignKey("Article")]
        public int ArticleId { get; set; }


        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }


        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string UserEmail { get; set; }

        // Navigation properties
        public virtual IdentityUser IdentityUser { get; set; }

        public virtual Article Article { get; set; }
    }
}
