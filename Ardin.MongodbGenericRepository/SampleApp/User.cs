using Ardin.MongodbGenericRepository;

namespace SampleApp
{
    [BsonCollection("User")]
    public class User : Document
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
