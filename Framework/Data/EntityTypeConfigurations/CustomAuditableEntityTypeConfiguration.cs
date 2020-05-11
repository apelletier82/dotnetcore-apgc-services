using Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public class CustomAuditableEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IAuditable
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {            
            builder.OwnsOne(o => o.Creation);
            builder.OwnsOne(o => o.LastChange);        
        }
    }
}