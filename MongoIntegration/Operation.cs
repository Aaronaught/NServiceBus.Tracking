using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace NServiceBus.Tracking.Mongo
{
    /// <summary>
    /// Implements an <see cref="IOperation"/> using a MongoDB collection.
    /// </summary>
    class Operation : IOperation
    {
        private readonly MongoCollection<OperationData> collection;
        private readonly ObjectId id;
        private readonly string originatingMessageType;
        private readonly DateTime whenStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation"/> class using the specified
        /// collection and operation data.
        /// </summary>
        /// <param name="collection">The collection where operation data is stored.</param>
        /// <param name="data">The data for the current operation.</param>
        public Operation(MongoCollection<OperationData> collection, OperationData data)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            this.collection = collection;
            this.id = data.Id;
            this.originatingMessageType = data.OriginatingMessageType;
            this.whenStarted = data.WhenStarted;
        }

        /// <inheritdoc />
        public IEnumerable<OperationStage> GetHistory()
        {
            var operation = collection.FindOneById(id);
            if (operation == null)
                return Enumerable.Empty<OperationStage>();
            return (operation.Stages ?? Enumerable.Empty<OperationStageData>())
                .Select(data => new OperationStage(data.ReceivedMessageType, data.WhenReceived));
        }

        /// <inheritdoc />
        public void Push(OperationStage nextStage)
        {
            if (nextStage == null)
                throw new ArgumentNullException("nextStage");
            collection.Update(
                Query<OperationData>.EQ(x => x.Id, id),
                Update<OperationData>.Push(x => x.Stages,
                    new OperationStageData
                    {
                        ReceivedMessageType = nextStage.ReceivedMessageType,
                        WhenReceived = nextStage.WhenReceived
                    })
            );
        }

        /// <inheritdoc />
        public bool WasMessageReceived(string messageTypeName)
        {
            var foundOperation = collection.FindOne(
                Query.And(
                    Query<OperationData>.EQ(x => x.Id, id),
                    Query<OperationData>.ElemMatch(x => x.Stages,
                        s => s.EQ(x => x.ReceivedMessageType, messageTypeName)
                    )
                )
            );
            return (foundOperation != null);
        }

        /// <inheritdoc />
        public string Id
        {
            get { return id.ToString(); }
        }

        /// <inheritdoc />
        public string OriginatingMessageType
        {
            get { return originatingMessageType; }
        }

        /// <inheritdoc />
        public DateTime WhenStarted
        {
            get { return whenStarted; }
        }
    }
}
