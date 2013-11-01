using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace NServiceBus.Tracking.Mongo
{
    /// <summary>
    /// Data Transfer Object for the <see cref="IOperation"/>.
    /// </summary>
    public class OperationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationData"/> class.
        /// </summary>
        public OperationData()
        {
            Stages = new List<OperationStageData>();
        }

        /// <inheritdoc cref="IOperation.Id" />
        public ObjectId Id { get; set; }

        /// <inheritdoc cref="IOperation.OriginatingMessageType" />
        public string OriginatingMessageType { get; set; }

        /// <inheritdoc cref="IOperation.WhenStarted" />
        public DateTime WhenStarted { get; set; }

        /// <summary>
        /// Gets or sets the list of stages for the current operation.
        /// </summary>
        public IList<OperationStageData> Stages { get; set; }
    }
}