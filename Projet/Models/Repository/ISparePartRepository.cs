
namespace Projet.Models.Repository
{
    public interface ISparePartRepository
    {
        IEnumerable<SparePart> GetAll();
        SparePart GetById(int id);
        void Add(SparePart sparePart);
        void Update(SparePart sparePart);
        void Delete(int id);
        void Save();
    }
}
