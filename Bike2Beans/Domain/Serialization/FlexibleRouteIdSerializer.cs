using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Bike2Beans.Domain.Serialization;

public sealed class FlexibleRouteIdSerializer : SerializerBase<string>
{
    public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return context.Reader.GetCurrentBsonType() switch
        {
            BsonType.ObjectId => context.Reader.ReadObjectId().ToString(),
            BsonType.String => context.Reader.ReadString(),
            BsonType.Binary => context.Reader
                .ReadBinaryData()
                .ToGuid(GuidRepresentation.Standard)
                .ToString(),
            BsonType.Null => ReadNull(context),
            _ => throw new FormatException("Route ids must be stored as an ObjectId, string, or Guid.")
        };
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            context.Writer.WriteNull();
            return;
        }

        if (ObjectId.TryParse(value, out var objectId))
        {
            context.Writer.WriteObjectId(objectId);
            return;
        }

        if (Guid.TryParse(value, out var guid))
        {
            context.Writer.WriteBinaryData(new BsonBinaryData(guid, GuidRepresentation.Standard));
            return;
        }

        context.Writer.WriteString(value);
    }

    private static string ReadNull(BsonDeserializationContext context)
    {
        context.Reader.ReadNull();
        return string.Empty;
    }
}
