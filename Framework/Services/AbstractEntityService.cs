using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Services.Abstract;

namespace Framework.Services
{
    /// <summary>
    /// Abstract class managing C, U, D operations on Entities
    /// </summary>
    /// <typeparam name="TEntity">Class, IEntity</typeparam>
    public abstract class AbstractEntityService<TEntity> :
        IAddEntityService<TEntity>, IUpdateEntityService<TEntity>, IDeleteEntityService<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Data store 
        /// </summary
        /// <value></value>
        protected CustomDBContext DBContext { get; private set; }

        public AbstractEntityService(CustomDBContext dbContext)
            => DBContext = dbContext;        

        public virtual TEntity Add(TEntity instance)
        {
            var result = DBContext.Add<TEntity>(instance).Entity;
            DBContext.SaveChanges(true);
            return result;
        }

        public async virtual Task<TEntity> AddAsync(TEntity instance, CancellationToken cancellationToken = default)
        {
            var result = await DBContext.AddAsync<TEntity>(instance, cancellationToken);
            await DBContext.SaveChangesAsync(true, cancellationToken);
            return result.Entity;
        }

        public virtual bool Delete(TEntity instance)
        {
            var result = DBContext.Remove<TEntity>(instance);
            DBContext.SaveChanges(true);
            return result != null;
        }

        public async virtual Task<bool> DeleteAsync(TEntity instance, CancellationToken cancellationToken = default)
        {
            var result = DBContext.Remove<TEntity>(instance);
            await DBContext.SaveChangesAsync(true, cancellationToken);
            return result != null;
        }        

        public virtual TEntity Update(TEntity instance)
        {            
            var result = DBContext.Update<TEntity>(instance);
            DBContext.SaveChanges(true);
            return result.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken = default)
        {
            var result = DBContext.Update<TEntity>(instance);
            await DBContext.SaveChangesAsync(true, cancellationToken);
            return result.Entity;
        }
    }
}