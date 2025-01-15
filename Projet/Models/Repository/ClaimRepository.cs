using Microsoft.EntityFrameworkCore;
using System;

namespace Projet.Models.Repository
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly AppDBContext _context;

        public ClaimRepository(AppDBContext context)
        {
            _context = context;
        }

        // Récupérer toutes les réclamations
        public async Task<IEnumerable<Claims>> GetAllClaimsAsync()
        {
            return await _context.Claims
                .Include(c => c.UserId)
                .Include(c => c.Article)
                .ToListAsync();
        }

        // Récupérer une réclamation par son ID
        public async Task<Claims?> GetClaimByIdAsync(int id)
        {
            return await _context.Claims
                .Include(c => c.UserId)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.ClaimId == id);
        }

        // Ajouter une nouvelle réclamation
        public async Task AddClaimAsync(Claims claim)
        {
            await _context.Claims.AddAsync(claim);
        }

        // Mettre à jour une réclamation existante
        public async Task UpdateClaimAsync(Claims claim)
        {
            _context.Claims.Update(claim);
        }

        // Supprimer une réclamation par son ID
        public async Task DeleteClaimAsync(int id)
        {
            var claim = await GetClaimByIdAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
            }
        }

        // Enregistrer les modifications dans la base de données
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
