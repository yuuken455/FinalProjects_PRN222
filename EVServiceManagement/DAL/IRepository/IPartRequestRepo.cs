using DAL.Entities;

namespace DAL.IRepository
{
    public interface IPartRequestRepo
    {
        Task<ICollection<PartRequest>> GetAll();
        Task AddPartRequest(PartRequest partRequest);   
    }
}
