using Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public class CustomRowVersionableEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IRowVersionable
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsRequired();
        }
    }
}