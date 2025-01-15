
namespace Projet.Models.Repository
{
    public interface IClaimRepository
    {

        Task<IEnumerable<Claims>> GetAllClaimsAsync();


        Task<Claims?> GetClaimByIdAsync(int id);


        Task AddClaimAsync(Claims claim);


        Task UpdateClaimAsync(Claims claim);


        Task DeleteClaimAsync(int id);


        Task SaveChangesAsync();
    }
}
