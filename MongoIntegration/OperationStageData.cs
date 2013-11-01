using System;

namespace NServiceBus.Tracking.Mongo
{
    /// <summary>
    /// Data Transfer Object for the <see cref="OperationStage"/>.
    /// </summary>
    public class OperationStageData
    {
        /// <inheritdoc cref="OperationStage.ReceivedMessageType" />
        public string ReceivedMessageType { get; set; }

        /// <inheritdoc cref="OperationStage.WhenReceived" />
        public DateTime WhenReceived { get; set; }
    }
}