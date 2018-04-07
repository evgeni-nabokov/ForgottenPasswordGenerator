using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Writer
{
    public class MongoDBWriter : IWriter
    {
        private const int BufferSizeInDocuments = 10_000;
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IList<WriteModel<BsonDocument>> _buffer;

        public Statistics Statistics { get; } = new Statistics();
        public string Destination { get; }

        public MongoDBWriter(string connectionString,
            string databaseName,
            string collectionName,
            string userName = null,
            string password = null,
            int bufferSizeInDocuments = BufferSizeInDocuments)
        {
            Destination = $"MongoDB ({databaseName}.{collectionName})";
            var client = new MongoClient(connectionString);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                client.Settings.Credential = MongoCredential.CreateCredential(databaseName, userName, password);
            }
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<BsonDocument>(collectionName);
            _buffer = new List<WriteModel<BsonDocument>>(bufferSizeInDocuments);
        }

        public void Write(string variation)
        {
            Statistics.Received++;
            var doc = new BsonDocument
            {
                { "_id", variation },
                { "isTried ", false }
            };
            _buffer.Add(new InsertOneModel<BsonDocument>(doc));
            if (_buffer.Count >= BufferSizeInDocuments)
            {
                WriteBuffer();
            }
        }

        public void Flush()
        {
            WriteBuffer();
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            Flush();
        }

        private void WriteBuffer()
        {
            try
            {
                var result = _collection.BulkWrite(_buffer, new BulkWriteOptions { IsOrdered = false });
                Statistics.Written += (ulong)result.InsertedCount;
            }
            catch (MongoBulkWriteException<BsonDocument> e)
            {
                Statistics.Written = (ulong)e.Result.InsertedCount;
            }
        }
    }
}
