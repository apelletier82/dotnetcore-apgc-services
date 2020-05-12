using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Exceptions;
using Framework.Services.Interfaces;

namespace Framework.Services
{
    /// <summary>
    /// Asbtract class managing C, U, D operation on Identifiable Entities
    /// </summary>
    /// <typeparam name="TEntity">Class, IIdentifiable</typeparam>
    public abstract class AbstractIdentifiableEntityService<TEntity> : AbstractEntityService<TEntity>,
        IGetIdentifiableEntityService<TEntity>, IDeleteIdentifiableEntityService<TEntity>
        where TEntity : class, IIdentifiable
    {
        public AbstractIdentifiableEntityService(AbstractDBContext dbContext) : base(dbContext)
        { }             

        public abstract TEntity Get(long id);

        public abstract Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);  

        public virtual bool Delete(long id)
        {
            var inst = Get(id); 
            if (inst == null)
                throw new EntityIdentityNotFoundException(id, typeof(TEntity).Name, "Not found");
            
            return Delete(inst);
        }

        public virtual async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var inst = await GetAsync(id, cancellationToken);
            if (inst == null)
                throw new EntityIdentityNotFoundException(id, typeof(TEntity).Name, "Not found");
            
            return await DeleteAsync(inst, cancellationToken);
        }              
    }
}