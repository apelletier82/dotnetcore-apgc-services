using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IGetIdentifiableEntityService<TEntity>: IGetEntityService<TEntity>
        where TEntity: class, IIdentifiable
    {
        TEntity Get(long id);
        Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default(CancellationToken));
    }
}