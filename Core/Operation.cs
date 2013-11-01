using System;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Holds information about the current state of an operation.
    /// </summary>
    public static class Operation
    {
        [ThreadStatic]
        private static IOperation current;

        /// <summary>
        /// Gets the operation that is currently in progress.
        /// </summary>
        public static IOperation Current
        {
            get { return current; }
            internal set { current = value; }
        }
    }
}