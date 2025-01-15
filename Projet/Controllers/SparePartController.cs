using Microsoft.AspNetCore.Mvc;
using Projet.Models;
using Projet.Models.Repository;

namespace Projet.Controllers
{
    public class SparePartController : Controller
    {
        readonly ISparePartRepository _sparePartRepository;

        public SparePartController(ISparePartRepository sparePartRepository)
        {
            _sparePartRepository = sparePartRepository;
        }

        // GET: SparePart
        public IActionResult Index()
        {
            IEnumerable<SparePart> spareParts = _sparePartRepository.GetAll();
            return View(spareParts);
        }

        // GET: SparePart/Details/5
        public IActionResult Details(int id)
        {
            SparePart sparePart = _sparePartRepository.GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart);
        }

        // GET: SparePart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SparePart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                _sparePartRepository.Add(sparePart);
                _sparePartRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(sparePart);
        }

        // GET: SparePart/Edit/5
        public IActionResult Edit(int id)
        {
            SparePart sparePart = _sparePartRepository.GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart);
        }

        // POST: SparePart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SparePart sparePart)
        {
            if (id != sparePart.SparePartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _sparePartRepository.Update(sparePart);
                    _sparePartRepository.Save();
                }
                catch
                {
                    // Gérer l'exception si nécessaire
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sparePart);
        }

        // GET: SparePart/Delete/5
        public IActionResult Delete(int id)
        {
            SparePart sparePart = _sparePartRepository.GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart);
        }

        // POST: SparePart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _sparePartRepository.Delete(id);
            _sparePartRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
