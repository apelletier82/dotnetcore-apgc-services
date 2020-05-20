namespace Framework.Entities
{
    public interface IIdentifiable : IEntity
    {
        long Id { get; }
    }
}