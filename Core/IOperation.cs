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
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <param name="messageTypeNames">An array containing the names of message types which
        /// must all be received in order for the operation to complete.</param>
        void CompleteAfter(params string[] messageTypeNames);

        /// <summary>
        /// Gets a sequence of all of the stages which have executed or are currently executing.
        /// </summary>
        IEnumerable<OperationStage> GetHistory();

        /// <summary>
        /// Checks whether or not the operation has finished (all required messages received).
        /// </summary>
        /// <returns><c>true</c> if the operation completed; <c>false</c> if it failed or is still
        /// running.</returns>
        bool IsCompleted();

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