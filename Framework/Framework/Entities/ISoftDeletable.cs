using Framework.Entities.Owned;

namespace Framework.Entities
{
    public interface ISoftDeletable: IEntity
    {
        bool Deleted { get; }
        Audit Deletion { get; }

        void Delete(string deletionUser);
        void Restore();
    }
}