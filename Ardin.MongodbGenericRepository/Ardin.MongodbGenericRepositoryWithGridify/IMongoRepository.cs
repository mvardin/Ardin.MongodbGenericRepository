using Gridify;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ardin.MongodbGenericRepositoryWithGridify
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        void InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);

        void ReplaceOne(TDocument document);

        Task ReplaceOneAsync(TDocument document);

        void UpdateOne(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update);

        Task UpdateOneAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update);

        void UpdateMany(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update);

        Task UpdateManyAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> update);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        void FindOneAndDelete(Expression<Func<TDocument, bool>> filterExpression);

        List<TDocument> GetWithGridify(GridifyQuery model);
    }
}
