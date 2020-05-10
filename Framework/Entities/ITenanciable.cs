namespace Framework.Entities
{
    public interface ITenanciable: IEntity
    {
        long TenantID { get; }        
    }
}