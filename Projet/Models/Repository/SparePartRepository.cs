// Assurez-vous que ceci pointe vers votre DbContext
namespace Projet.Models.Repository
{
    public class SparePartRepository : ISparePartRepository
    {
        readonly AppDBContext _context;

        public SparePartRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<SparePart> GetAll()
        {
            return _context.SpareParts.ToList();
        }

        public SparePart GetById(int id)
        {
            return _context.SpareParts.FirstOrDefault(sp => sp.SparePartId == id);
        }

        public void Add(SparePart sparePart)
        {
            _context.SpareParts.Add(sparePart);
        }

        public void Update(SparePart sparePart)
        {
            _context.SpareParts.Update(sparePart);
        }

        public void Delete(int id)
        {
            var sparePart = GetById(id);
            if (sparePart != null)
            {
                _context.SpareParts.Remove(sparePart);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
