using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Contains helper methods for the <see cref="IOperation"/> interface.
    /// </summary>
    public static class OperationExtensions
    {
        /// <summary>
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <param name="operation">The operation to configure.</param>
        /// <param name="types">An array of types containing the message types that must be
        /// received for the operation to be considered complete.</param>
        public static void CompleteAfter(this IOperation operation, params Type[] types)
        {
            var typeNames = (types ?? Enumerable.Empty<Type>())
                .Select(t => t.FullName)
                .ToArray();
            operation.CompleteAfter(typeNames);
        }

        /// <summary>
        /// Indicates that the operation should be considered complete after a message is received.
        /// </summary>
        /// <typeparam name="T">The message type which triggers completion of the
        /// operation.</typeparam>
        /// <param name="operation"></param>
        public static void CompleteAfter<T>(this IOperation operation)
        {
            CompleteAfter(operation, typeof(T));
        }

        /// <summary>
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <typeparam name="T1">The first message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T2">The second message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <param name="operation">The operation to configure.</param>
        public static void CompleteAfter<T1, T2>(this IOperation operation)
        {
            CompleteAfter(operation, typeof(T1), typeof(T2));
        }

        /// <summary>
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <typeparam name="T1">The first message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T2">The second message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T3">The third message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <param name="operation">The operation to configure.</param>
        public static void CompleteAfter<T1, T2, T3>(this IOperation operation)
        {
            CompleteAfter(operation, typeof(T1), typeof(T2), typeof(T3));
        }

        /// <summary>
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <typeparam name="T1">The first message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T2">The second message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T3">The third message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T4">The fourth message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <param name="operation">The operation to configure.</param>
        public static void CompleteAfter<T1, T2, T3, T4>(this IOperation operation)
        {
            CompleteAfter(operation, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        /// <summary>
        /// Indicates that the operation should be considered complete after all of a set of
        /// messages are retrieved.
        /// </summary>
        /// <typeparam name="T1">The first message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T2">The second message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T3">The third message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T4">The fourth message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <typeparam name="T5">The fifth message type that must be received in order for the
        /// operation to be considered complete.</typeparam>
        /// <param name="operation">The operation to configure.</param>
        public static void CompleteAfter<T1, T2, T3, T4, T5>(this IOperation operation)
        {
            CompleteAfter(operation, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }
    }
}