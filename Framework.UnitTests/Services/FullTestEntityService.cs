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

        public override FullTestEntity FindRowVersion(long id, byte[] rowVersion)
            =>  DBContext.FullTestEntities
                    .Where(e => e.Id.Equals(id) && e.RowVersion.SequenceEqual(rowVersion))
                    .SingleOrDefault();

        public override async Task<FullTestEntity> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default)
            => await DBContext.FullTestEntities
                .Where(e => e.Id.Equals(id) && e.RowVersion.SequenceEqual(rowVersion))
                .SingleOrDefaultAsync();            
        
        public override FullTestEntity Get(long id)
            => DBContext?.FullTestEntities.Find(id);
        
        public override async Task<FullTestEntity> GetAsync(long id, CancellationToken cancellationToken = default)
            =>  DBContext != null ? await DBContext.FullTestEntities.FindAsync(id, cancellationToken) : null;

        public IEnumerable<FullTestEntity> GetList()
            => DBContext?.FullTestEntities.ToList();

        public async Task<IEnumerable<FullTestEntity>> GetListAsync()
            => await DBContext?.FullTestEntities.ToListAsync();
    }
}