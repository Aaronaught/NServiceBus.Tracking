using System;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Represents one stage of a long-running operation.
    /// </summary>
    public class OperationStage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationStage"/> class with the specified
        /// message type name and receipt date.
        /// </summary>
        /// <param name="receivedMessageType">The name of the message type that was received.</param>
        /// <param name="whenReceived">The date and time when the message was received.</param>
        public OperationStage(string receivedMessageType, DateTime whenReceived)
        {
            if (string.IsNullOrEmpty(receivedMessageType))
                throw new ArgumentException("Parameter 'receivedMessageType' cannot be null or empty.",
                    "receivedMessageType");
            this.ReceivedMessageType = receivedMessageType;
            this.WhenReceived = whenReceived;
        }

        /// <summary>
        /// Gets the name of the message type that was received.
        /// </summary>
        public string ReceivedMessageType { get; private set; }

        /// <summary>
        /// Gets the date and time when the message was received.
        /// </summary>
        public DateTime WhenReceived { get; private set; }
    }
}