using System;
using Framework.Entities;
using Framework.Entities.Owned;
using Framework.Models;

namespace Framework.UnitTests.Entities
{
    public class FullTestEntity : IIdentifiable, IRowVersionable,
        IAuditable, ISoftDeletable, ITenanciable
    {
        public long Id { get; private set; }
        public byte[] RowVersion { get; private set; }
        public Audit Creation { get; private set; }
        public Audit LastChange { get; private set; }
        public bool Deleted { get; private set; }
        public Audit Deletion { get; private set; }
        public long TenantID { get; private set; }
        public string Name { get; private set; }
        public DateTime Date { get; private set; }

        private FullTestEntity()
        { }

        public FullTestEntity(ITenant tenant, string name, DateTime date)
        {            
            TenantID = tenant != null ? tenant.Id : 0;
            Deleted = false;
            Creation = new Audit();
            LastChange = new Audit();
            Deletion = new Audit();

            UpdateName(name);
            UpdateDate(date);
        }

        public FullTestEntity(ITenant tenant, string name) : this(tenant, name, DateTime.Now)
        { }

        public FullTestEntity(ITenant tenant, DateTime date) : this(tenant, "", date)
        { }

        public FullTestEntity(ITenant tenant) : this(tenant, "", DateTime.Now)
        { }

        public void UpdateName(string newName)
            => Name = newName;

        public void UpdateDate(DateTime date)
            => Date = date.Date;

        public void Delete(string deletionUser)
        {
            Deleted = true;
            Deletion.DoAudit(deletionUser);
        }

        public void Restore()
            => Deleted = false;
    }
}