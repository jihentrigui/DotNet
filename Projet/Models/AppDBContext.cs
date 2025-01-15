using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Projet.Models
{
    public class AppDBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        // Ajoutez ici vos autres DbSet pour les entités personnalisées

        public DbSet<Claims> Claims { get; set; }
        public DbSet<TechnicalIntervention> TechnicalInterventions { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>()
       .HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<Article>()
                .Property(a => a.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SparePart>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<TechnicalIntervention>()
                .Property(t => t.LaborCost)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<TechnicalIntervention>()
                .Property(t => t.TotalCost)
                .HasColumnType("decimal(18, 2)");
        }



    }

}

