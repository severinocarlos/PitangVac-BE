using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {

        Task<TEntity> Save(TEntity entity);
        Task SaveAll(IEnumerable<TEntity> entities);
        Task<TEntity> Update(TEntity entity);
        Task Remove(TEntity entity);
        Task RemoveAll(List<TEntity> entities);
        Task RemoveById(object entityId);
        Task<TEntity?> GetById(object id);
        Task<List<TEntity>> All();
    }
}
