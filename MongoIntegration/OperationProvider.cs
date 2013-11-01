using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NServiceBus.Tracking.Mongo
{
    /// <summary>
    /// Implements an <see cref="IOperationProvider"/> using a MongoDB collection.
    /// </summary>
    public class OperationProvider : IOperationProvider
    {
        private readonly MongoCollection<OperationData> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationProvider"/> class using the
        /// specified collection.
        /// </summary>
        /// <param name="collection">The collection where operation data is stored.</param>
        public OperationProvider(MongoCollection<OperationData> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            this.collection = collection;
        }

        /// <inheritdoc />
        public IOperation Find(string id)
        {
            ObjectId parsedId;
            if (!ObjectId.TryParse(id, out parsedId))
                return null;
            var operationData = collection.FindOneById(parsedId);
            return (operationData != null) ? new Operation(collection, operationData) : null;
        }

        /// <inheritdoc />
        public IOperation Start(string originatingMessageType)
        {
            var data = new OperationData
            {
                OriginatingMessageType = originatingMessageType,
                WhenStarted = DateTime.Now
            };
            collection.Save(data);
            return new Operation(collection, data);
        }
    }
}