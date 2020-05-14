using Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public class CustomSoftDeletableTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, ISoftDeletable        
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.OwnsOne(o => o.Deletion).Property(p => p.User).HasMaxLength(60);
            builder.HasIndex(i => i.Deleted);            
        }
    }
}