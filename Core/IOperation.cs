using System;
using System.Collections.Generic;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Encapsulates a persistent, tracked operation involving multiple NServiceBus messages.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Gets a sequence of all of the stages which have executed or are currently executing.
        /// </summary>
        IEnumerable<OperationStage> GetHistory();

        /// <summary>
        /// Pushes a new item at the end of the operation history.
        /// </summary>
        /// <param name="nextStage">The information about the stage of the operation which is
        /// currently executing.</param>
        void Push(OperationStage nextStage);

        /// <summary>
        /// Tests if a specific type of message was received within the context of the current
        /// operation.
        /// </summary>
        /// <param name="messageTypeName">The name of the message type to look for.</param>
        /// <returns><c>true</c> if a message of the type named <paramref name="messageTypeName"/>
        /// was received during the operation; otherwise, <c>false</c>.</returns>
        bool WasMessageReceived(string messageTypeName);

        /// <summary>
        /// Gets the unique identifier of the operation, which can be used to retrieve it again
        /// from an <see cref="IOperationProvider"/>.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the message type that originated the operation.
        /// </summary>
        string OriginatingMessageType { get; }

        /// <summary>
        /// Gets the date and time when the operation was started.
        /// </summary>
        DateTime WhenStarted { get; }
    }
}