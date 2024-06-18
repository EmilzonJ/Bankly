using MongoDB.Bson;
using Newtonsoft.Json;

namespace Infrastructure.Utils;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override ObjectId ReadJson(
        JsonReader reader,
        Type objectType,
        ObjectId existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        if (reader.Value == null)
        {
            return ObjectId.Empty;
        }

        var value = reader.Value.ToString();
        return ObjectId.Parse(value);
    }
}

