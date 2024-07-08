using Microsoft.EntityFrameworkCore;
using PitangVac.Entity.Entities;
using PitangVac.Repository;
using PitangVac.Repository.Interface.IRepositories;

namespace TaskControl.Repository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {

        protected readonly DatabaseContext _dbContext;

        protected virtual DbSet<TEntity> EntitySet { get; }

        public BaseRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            EntitySet = _dbContext.Set<TEntity>();
        }

        public Task<List<TEntity>> All() => EntitySet.ToListAsync();

        public async Task<TEntity?> GetById(object entityId) => await EntitySet.FindAsync(entityId);

        public async Task<TEntity> Save(TEntity entity)
        {
            var entityEntry = await EntitySet.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task SaveAll(IEnumerable<TEntity> entities)
        {
            await EntitySet.AddRangeAsync(entities);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(TEntity entity)
        {
            EntitySet.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAll(List<TEntity> entities)
        {
            EntitySet.RemoveRange(entities);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveById(object entityId)
        {
            var entity = await EntitySet.FindAsync(entityId);

            if (entity != null)
                await Remove(entity);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var entityEntry = EntitySet.Update(entity);

            await _dbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }
    }
}