using MongoDB.Bson;

namespace Application.Extensions;

public static class MongoIdConversion
{
    public static Result<ObjectId> ToObjectId(this string id)
        => !ObjectId.TryParse(id, out var objectId)
            ? Result.Failure<ObjectId>(Error.Validation("Validations.Id", "The id is not a valid ObjectId."))
            : objectId;
}
