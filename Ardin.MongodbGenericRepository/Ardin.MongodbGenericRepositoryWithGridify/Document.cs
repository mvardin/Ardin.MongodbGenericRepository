using MongoDB.Bson;

namespace Ardin.MongodbGenericRepositoryWithGridify
{
    public abstract class Document : IDocument
    {
        public string Id { get; set; } = ObjectId.Empty.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
