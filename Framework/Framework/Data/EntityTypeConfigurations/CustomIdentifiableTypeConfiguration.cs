using Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public class CustomIdentifiableTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : class, IIdentifiable
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}