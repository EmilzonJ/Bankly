using MongoDB.Bson;

namespace Domain.Base;

public interface ICollectionBase
{
    public ObjectId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public abstract class CollectionBase : ICollectionBase
{
    public ObjectId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
