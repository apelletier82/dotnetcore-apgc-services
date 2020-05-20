using Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public abstract class CustomAbstractEntityTypeConfiguration : IEntityTypeConfiguration<IEntity>
    {
        public abstract void Configure(EntityTypeBuilder<IEntity> builder);        
    }
}