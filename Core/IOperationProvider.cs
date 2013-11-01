using System;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Provides methods to create operations or retrieve running operations.
    /// </summary>
    public interface IOperationProvider
    {
        /// <summary>
        /// Attempts to retrieve an operation that has already started.
        /// </summary>
        /// <param name="id">The unique identifier of the operation.</param>
        /// <returns>The <see cref="IOperation"/> with the specified <paramref name="id"/>, or
        /// <c>null</c> if no matching operation can be found.</returns>
        IOperation Find(string id);

        /// <summary>
        /// Starts a new operation.
        /// </summary>
        /// <param name="originatingMessageType">The name of the type of the message which
        /// originated the operation.</param>
        /// <returns>A new <see cref="IOperation"/> which can be used to track the progress of a
        /// chain of messages and handlers.</returns>
        IOperation Start(string originatingMessageType);
    }
}