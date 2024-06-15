using MongoDB.Bson;

namespace Domain.Base;

public interface ICollectionBase
{
    public ObjectId Id { get; set; }
}

public abstract class CollectionBase : ICollectionBase
{
    public ObjectId Id { get; set; }
}
