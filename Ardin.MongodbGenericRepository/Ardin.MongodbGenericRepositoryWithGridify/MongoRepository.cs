using Gridify;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ardin.MongodbGenericRepositoryWithGridify
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            var mongoConnectionString = root.GetSection("MongoConnectionString").Value;
            var mongoDatabase = root.GetSection("MongoDatabase").Value;
            var database = new MongoClient(mongoConnectionString).GetDatabase(mongoDatabase);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TDocument document)
        {
            if (document.Id == ObjectId.Empty.ToString())
            {
                document.Id = ObjectId.GenerateNewId().ToString();
                document.CreatedAt = DateTime.Now;
            }
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public virtual void UpdateOne(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update)
        {
            _collection.UpdateOne(filterExpression, update);
        }

        public virtual Task UpdateOneAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update)
        {
            return Task.Run(() => _collection.UpdateOneAsync<TDocument>(filterExpression, update));
        }

        public virtual void UpdateMany(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update)
        {
            _collection.UpdateMany<TDocument>(filterExpression, update);
        }

        public virtual Task UpdateManyAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update)
        {
            return Task.Run(() => _collection.UpdateManyAsync<TDocument>(filterExpression, update));
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

        public virtual void FindOneAndDelete(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public List<TDocument> GetWithGridify(GridifyQuery model)
        {
            if (model.Page == -1)
                return _collection.AsQueryable().OrderByDescending(c => c.CreatedAt).ToList();

            else
                return _collection.AsQueryable().Gridify(model).Data.OrderByDescending(c => c.CreatedAt).ToList();
        }
    }
}
