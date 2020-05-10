using Framework.Entities;
using Framework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Data.EntityTypeConfigurations
{
    public class CustomTenanciableEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, ITenanciable
    {
        private ITenant _tenant;

        public CustomTenanciableEntityTypeConfiguration(ITenant tenant)
        {
            _tenant = tenant;
        }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(q => q.TenantID == _tenant.Id);
            builder.HasIndex(i => i.TenantID);
        }
    }
}