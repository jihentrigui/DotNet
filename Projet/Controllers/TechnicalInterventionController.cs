using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Models;
using Projet.ViewModels;

namespace Projet.Controllers
{
    public class TechnicalInterventionController : Controller
    {
        private readonly AppDBContext _context;

        public TechnicalInterventionController(AppDBContext context)
        {
            _context = context;
        }

        // Liste des interventions
        public async Task<IActionResult> Index()
        {
            var interventions = await _context.TechnicalInterventions
                .Include(t => t.Claim)
                .Include(t => t.SparePartsUsed)
                .ToListAsync();

            return View(interventions);
        }

        // Ajouter une intervention (GET)
        public IActionResult Create()
        {
            ViewBag.Claims = new SelectList(_context.Claims.Include(c => c.Article), "ClaimId", "Description");
            ViewBag.SpareParts = new SelectList(_context.SpareParts, "SparePartId", "Name");
            return View();
        }

        // Ajouter une intervention (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechnicalInterventionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Charger la réclamation et l'article associé
                    var claim = await _context.Claims.Include(c => c.Article).FirstOrDefaultAsync(c => c.ClaimId == model.ClaimId);
                    if (claim == null)
                    {
                        ModelState.AddModelError("ClaimId", "La réclamation sélectionnée n'existe pas.");
                        return View(model);
                    }

                    var intervention = new TechnicalIntervention
                    {
                        ClaimId = model.ClaimId,
                        InterventionDate = model.InterventionDate,
                        IsWarranty = model.IsWarranty,
                        LaborCost = model.LaborCost,
                    };

                    // Charger les pièces utilisées
                    intervention.SparePartsUsed = _context.SpareParts
                        .Where(sp => model.SparePartIds.Contains(sp.SparePartId))
                        .ToList();

                    // Déterminer si l'intervention est gratuite ou facturée
                    if (claim.Article.IsUnderWarranty)
                    {
                        intervention.IsWarranty = true;
                        intervention.TotalCost = 0; // Gratuit
                    }
                    else
                    {
                        intervention.IsWarranty = false;
                        var sparePartsCost = intervention.SparePartsUsed.Sum(sp => sp.Price);
                        intervention.TotalCost = sparePartsCost + intervention.LaborCost;
                    }

                    _context.TechnicalInterventions.Add(intervention);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Intervention technique ajoutée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'ajout de l'intervention : {ex.Message}");
                    ModelState.AddModelError("", "Une erreur s'est produite. Veuillez réessayer.");
                }
            }

            ViewBag.Claims = new SelectList(_context.Claims.Include(c => c.Article), "ClaimId", "Description", model.ClaimId);
            ViewBag.SpareParts = new SelectList(_context.SpareParts, "SparePartId", "Name", model.SparePartIds);
            return View(model);
        }

        // Modifier une intervention (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intervention = await _context.TechnicalInterventions
                .Include(t => t.SparePartsUsed)
                .FirstOrDefaultAsync(t => t.TechnicalInterventionId == id);

            if (intervention == null)
            {
                return NotFound();
            }

            var model = new TechnicalInterventionViewModel
            {
                TechnicalInterventionId = intervention.TechnicalInterventionId,
                ClaimId = intervention.ClaimId,
                InterventionDate = intervention.InterventionDate,
                IsWarranty = intervention.IsWarranty,
                LaborCost = intervention.LaborCost,
                SparePartIds = intervention.SparePartsUsed.Select(sp => sp.SparePartId).ToList(),
                TotalCost = intervention.TotalCost
            };

            ViewBag.Claims = new SelectList(_context.Claims.Include(c => c.Article), "ClaimId", "Description", model.ClaimId);
            ViewBag.SpareParts = new SelectList(_context.SpareParts, "SparePartId", "Name", model.SparePartIds);
            return View(model);
        }

        // Modifier une intervention (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TechnicalInterventionViewModel model)
        {
            if (id != model.TechnicalInterventionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var intervention = await _context.TechnicalInterventions
                        .Include(t => t.SparePartsUsed)
                        .FirstOrDefaultAsync(t => t.TechnicalInterventionId == id);

                    if (intervention == null)
                    {
                        return NotFound();
                    }

                    var claim = await _context.Claims.Include(c => c.Article).FirstOrDefaultAsync(c => c.ClaimId == model.ClaimId);
                    if (claim == null)
                    {
                        ModelState.AddModelError("ClaimId", "La réclamation sélectionnée n'existe pas.");
                        return View(model);
                    }

                    intervention.ClaimId = model.ClaimId;
                    intervention.InterventionDate = model.InterventionDate;
                    intervention.IsWarranty = claim.Article.IsUnderWarranty;
                    intervention.LaborCost = model.LaborCost;

                    intervention.SparePartsUsed = _context.SpareParts
                        .Where(sp => model.SparePartIds.Contains(sp.SparePartId))
                        .ToList();

                    // Calculer le coût total
                    if (claim.Article.IsUnderWarranty)
                    {
                        intervention.TotalCost = 0; // Gratuit
                    }
                    else
                    {
                        var sparePartsCost = intervention.SparePartsUsed.Sum(sp => sp.Price);
                        intervention.TotalCost = sparePartsCost + intervention.LaborCost;
                    }

                    _context.Update(intervention);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Intervention technique modifiée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la modification : {ex.Message}");
                    ModelState.AddModelError("", "Une erreur s'est produite. Veuillez réessayer.");
                }
            }

            ViewBag.Claims = new SelectList(_context.Claims.Include(c => c.Article), "ClaimId", "Description", model.ClaimId);
            ViewBag.SpareParts = new SelectList(_context.SpareParts, "SparePartId", "Name", model.SparePartIds);
            return View(model);
        }

        // Supprimer une intervention (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intervention = await _context.TechnicalInterventions
                .Include(t => t.Claim)
                .FirstOrDefaultAsync(t => t.TechnicalInterventionId == id);

            if (intervention == null)
            {
                return NotFound();
            }

            return View(intervention);
        }

        // Supprimer une intervention (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intervention = await _context.TechnicalInterventions.FindAsync(id);
            if (intervention != null)
            {
                _context.TechnicalInterventions.Remove(intervention);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Intervention technique supprimée avec succès.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
