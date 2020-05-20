using Framework.Entities;

namespace Framework.Models
{
    public interface ITenant : IIdentifiable
    {
        string Host { get; }
    }
}