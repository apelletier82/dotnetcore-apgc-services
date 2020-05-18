using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Services.Interfaces;
using Framework.UnitTests.Data.DBContexts;
using Framework.UnitTests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Framework.UnitTests.Services
{
    public class FullTestEntityService : AbstractRowVersionableEntityService<FullTestEntity, DBContext>
    {
        public FullTestEntityService(DBContext dbContext) : base(dbContext)
        { }

        public override bool FindRowVersion(long id, byte[] rowVersion)
            =>  DBContext.FullTestEntities
                    .Where(e => e.Id == id && e.RowVersion == rowVersion)
                    .SingleOrDefault() != null;

        public override bool FindRowVersion(FullTestEntity instance)
            => FindRowVersion(instance.Id, instance.RowVersion);

        public override async Task<bool> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellationToken = default)
            => await DBContext.FullTestEntities
                .Where(e => e.Id == id && e.RowVersion == rowVersion)
                .SingleOrDefaultAsync(cancellationToken) != null;

        public override async Task<bool> FindRowVersionAsync(FullTestEntity instance, CancellationToken cancellationToken = default)
            => await FindRowVersionAsync(instance.Id, instance.RowVersion, cancellationToken);

        public FullTestEntity Get(long id)
            => DBContext?.FullTestEntities.Find(id);
        
        public async Task<FullTestEntity> GetAsync(long id, CancellationToken cancellationToken = default)
            =>  DBContext != null ? await DBContext.FullTestEntities.FindAsync(id, cancellationToken) : null;

        public IEnumerable<FullTestEntity> GetList(bool includeDeleted = false)
            => DBContext?.FullTestEntities
                .Where(e => (includeDeleted || e.Deleted == false))
                .ToList();

        public IEnumerable<FullTestEntity> GetDeletedList()
            => DBContext?.FullTestEntities
                .Where(e => e.Deleted == true)
                .ToList();

        public async Task<IEnumerable<FullTestEntity>> GetListAsync(bool includeDeleted = false)
            => await DBContext?.FullTestEntities
                .Where(e => (includeDeleted || e.Deleted == false))
                .ToListAsync();

        public async Task<IEnumerable<FullTestEntity>> GetDeletedListAsync()
            => await DBContext?.FullTestEntities
                .Where(e => e.Deleted == true)
                .ToListAsync();

        public int? Count()
            => DBContext?.FullTestEntities.Count();
        
        public async Task<int> CountAsync()
            => await DBContext?.FullTestEntities.CountAsync();
    }
}