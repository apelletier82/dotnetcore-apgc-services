using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Services.Abstract;

namespace Framework.Services
{
    public abstract class AbstractIdentifiableEntityService<TEntity> : AbstractEntityService<TEntity>,
        IGetIdentifiableEntityService<TEntity>, IDeleteIdentifiableEntityService<TEntity>
        where TEntity : class, IIdentifiable
    {
        public AbstractIdentifiableEntityService(CustomDBContext dbContext) : base(dbContext)
        { }
        
        public virtual bool Delete(long id)
            => Delete(Get(id));        

        public virtual async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
            => await DeleteAsync(await GetAsync(id, cancellationToken), cancellationToken);        

        public abstract TEntity Get(long id);

        public abstract Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);        
    }
}