using DAL.Entities;

namespace DAL.IRepository
{
    public interface IPartRepo
    {
        Task<ICollection<Part>> GetAll();
        Task<Part?> GetPartById(int id);
    }
}
