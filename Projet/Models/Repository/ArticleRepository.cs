using Microsoft.EntityFrameworkCore;


namespace Projet.Models.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        readonly AppDBContext _context;

        public ArticleRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _context.Articles.Update(article);
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await GetArticleByIdAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
