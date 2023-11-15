using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ardin.MongodbGenericRepositoryWithGridify
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string Id { get; set; } 
        DateTime CreatedAt { get; set; }
    }
}
