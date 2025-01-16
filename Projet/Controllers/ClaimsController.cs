using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Models;
using Projet.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Projet.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClaimsController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Client")]
        public IActionResult Create()
        {
            // Charger tous les articles disponibles
            ViewBag.Articles = new SelectList(_context.Articles, "ArticleId", "Name");
            return View();
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
          




            if (ModelState.IsValid)
            {
                try
                {
                    // Récupérer l'utilisateur connecté
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return Unauthorized("Vous devez être connecté pour effectuer cette action.");
                    }



                    // Vérification de l'article
                    var article = await _context.Articles.FindAsync(model.ArticleId);
                    if (article == null)
                    {
                        ModelState.AddModelError("ArticleId", "L'article sélectionné n'existe pas.");
                        return View(model);
                    }

                    // Créer et sauvegarder le Claim
                    var claim = new Claims
                    {
                        UserEmail = user?.Email, // Associer l'email à chaque réclamation
                        UserId = user.Id, // Associer au IdentityUser
                        ArticleId = model.ArticleId,
                        Description = model.Description,
                        Status = model.Status, // Statut par défaut
                        Date = DateTime.Now,
                    };

                    _context.Claims.Add(claim);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Votre réclamation a été ajoutée avec succès.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    using (var writer = new StreamWriter("log.txt", true))
                    {
                        // Afficher l'exception principale
                        Console.WriteLine($"Erreur lors de l'ajout de la réclamation : {ex.Message}");

                        // Vérifier si une inner exception existe et l'afficher
                        if (ex.InnerException != null)
                        {
                            writer.WriteLine($"Inner Exception : {ex.InnerException.Message}");
                        }

                                       }

                    ModelState.AddModelError("", "Une erreur s'est produite. Veuillez réessayer.");
                }
            }

            // Recharger la liste des articles en cas d'erreur
            ViewBag.Articles = new SelectList(_context.Articles, "ArticleId", "Name");
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Récupérer l'utilisateur connecté
            var user = await _userManager.GetUserAsync(User);

            // Vérifier si l'utilisateur est un responsable
            var isResponsible = await _userManager.IsInRoleAsync(user, "Responsable");

            IQueryable<Claims> claimsQuery = _context.Claims.Include(c => c.Article);

            // Si l'utilisateur est un client, ne montrer que ses propres réclamations
            if (!isResponsible)
            {
                claimsQuery = claimsQuery.Where(c => c.UserId == user.Id);
            }

            // Récupérer les réclamations
            var claims = await claimsQuery.ToListAsync();

            // Ajouter l'email de l'utilisateur aux données de la vue
            foreach (var claim in claims)
            {
                var claimUser = await _userManager.FindByIdAsync(claim.UserId);
                claim.UserEmail = claimUser?.Email; // Associer l'email à chaque réclamation
            }

            return View(claims);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            ViewBag.Articles = new SelectList(_context.Articles, "ArticleId", "Name", claim.ArticleId);

            var model = new ClaimViewModel
            {
                ArticleId = claim.ArticleId,
                Description = claim.Description,
                Status = claim.Status,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claim = await _context.Claims.FindAsync(id);
                    if (claim == null)
                    {
                        return NotFound();
                    }

                    claim.ArticleId = model.ArticleId;
                    claim.Description = model.Description;
                    claim.Status = model.Status;

                    _context.Update(claim);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Réclamation modifiée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la modification : {ex.Message}");
                    ModelState.AddModelError("", "Une erreur s'est produite. Veuillez réessayer.");
                }
            }

            ViewBag.Articles = new SelectList(_context.Articles, "ArticleId", "Name", model.ArticleId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.Article)
                .Include(c => c.IdentityUser) // Inclure les informations sur l'article

                .FirstOrDefaultAsync(m => m.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Réclamation supprimée avec succès.";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.Article) // Inclure les informations sur l'article
                .FirstOrDefaultAsync(m => m.ClaimId == id);

            if (claim == null)
            {
                return NotFound();
            }

            // Récupérer l'email de l'utilisateur
            var user = await _userManager.FindByIdAsync(claim.UserId);
            claim.UserEmail = user?.Email; // Ajouter l'email à la réclamation

            return View(claim);
        }
    }
}
