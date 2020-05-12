using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Interfaces
{
    public interface IAddEntityService<TEntity> 
        where TEntity: class, IEntity 
    {
         TEntity Add(TEntity instance);
         Task<TEntity> AddAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}