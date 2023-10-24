using MongoDB.Bson;

namespace Ardin.MongodbGenericRepository
{
    public abstract class Document : IDocument
    {
        public string _id { get; set; } = ObjectId.Empty.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
