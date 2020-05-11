using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IDeleteIdentifiableEntityService<TEntity>: IDeleteEntityService<TEntity>
        where TEntity: class, IIdentifiable
    {
         bool Delete(long id);
         Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken));
    }
}