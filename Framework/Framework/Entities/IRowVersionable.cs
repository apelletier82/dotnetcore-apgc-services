namespace Framework.Entities
{
    public interface IRowVersionable: IEntity
    {
        byte[] RowVersion { get; }
    }
}