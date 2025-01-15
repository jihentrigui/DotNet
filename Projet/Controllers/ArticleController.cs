using Microsoft.AspNetCore.Mvc;
using Projet.Models;
using Projet.Models.Repository;

namespace Projet.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // GET: /Article
        // Liste de tous les articles
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            return View(articles);
        }

        // GET: /Article/Details/{id}
        // Affiche les détails d'un article spécifique
        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // GET: /Article/Create
        // Affiche le formulaire pour créer un nouvel article
        public IActionResult Create()
        {
            return View(new Article());
        }

        // POST: /Article/Create
        // Crée un nouvel article
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                await _articleRepository.AddArticleAsync(article);
                await _articleRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: /Article/Edit/{id}
        // Affiche le formulaire pour modifier un article existant
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: /Article/Edit/{id}
        // Modifie un article existant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _articleRepository.UpdateArticleAsync(article);
                await _articleRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: /Article/Delete/{id}
        // Confirme la suppression d'un article
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: /Article/Delete/{id}
        // Supprime un article
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _articleRepository.DeleteArticleAsync(id);
            await _articleRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
