using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Exceptions;
using Framework.Services.Interfaces;

namespace Framework.Services
{
    /// <summary>
    /// <para>Asbtract class managing C, U, D operation on Identifiable Entities.</para>
    /// <para>Check that entity id exists in datastore before updating / deleting.</para>
    /// </summary>    
    /// <typeparam name="TEntity">Class, IIdentifiable</typeparam>
    /// <typeparam name="TDBContext">Class of AbstractDBContext</typeparam>
    public abstract class AbstractIdentifiableEntityService<TEntity, TDBContext> : AbstractEntityService<TEntity, TDBContext>,
        IGetIdentifiableEntityService<TEntity>, IDeleteIdentifiableEntityService<TEntity>
        where TEntity : class, IIdentifiable
        where TDBContext: AbstractDBContext
    {
        public AbstractIdentifiableEntityService(TDBContext dbContext) : base(dbContext)
        { }             

        public abstract TEntity Get(long id);

        public abstract Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);  

        /// <exception cref="Framework.Exceptions.EntityIdentityNotFoundException">Entity's Id not found in the DataStore</exception>
        public virtual bool Delete(long id)
        {
            var inst = Get(id); 
            if (inst == null)
                throw new EntityIdentityNotFoundException(id, typeof(TEntity).Name, "Not found");
            
            return Delete(inst);
        }
        
        /// <exception cref="Framework.Exceptions.EntityIdentityNotFoundException">Entity's Id not found in the DataStore</exception>
        public virtual async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var inst = await GetAsync(id, cancellationToken);
            if (inst == null)
                throw new EntityIdentityNotFoundException(id, typeof(TEntity).Name, "Not found");
            
            return await DeleteAsync(inst, cancellationToken);
        }              
    }
}