using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IDeleteEntityService<TEntity> 
        where TEntity: class, IEntity
    {
         bool Delete(TEntity instance);
         Task<bool> DeleteAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}