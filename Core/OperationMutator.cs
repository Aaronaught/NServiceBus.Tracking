using System;
using System.Collections.Generic;
using log4net;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Transport;
using NServiceBus.UnitOfWork;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Embeds and propagates operation information in incoming and outgoing messages.
    /// </summary>
    public class OperationMutator : IMutateOutgoingTransportMessages, IManageUnitsOfWork
    {
        /// <summary>
        /// The name of the message header that contains the Operation ID.
        /// </summary>
        internal const string OperationIdHeader = "OperationId";

        private readonly IBus bus;
        private readonly IOperationProvider operationProvider;
        private readonly ILog log = LogManager.GetLogger(typeof(OperationMutator));

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMutator"/> class using the
        /// specified bus instance and operation provider.
        /// </summary>
        /// <param name="bus">The current NServiceBus instance.</param>
        /// <param name="operationProvider">The provider to use for creating and retrieving
        /// operations.</param>
        public OperationMutator(IBus bus, IOperationProvider operationProvider)
        {
            if (bus == null)
                throw new ArgumentNullException("bus");
            if (operationProvider == null)
                throw new ArgumentNullException("operationProvider");
            this.bus = bus;
            this.operationProvider = operationProvider;
        }

        /// <inheritdoc />
        public void Begin()
        {
        }

        /// <inheritdoc />
        public void End(Exception ex = null)
        {
            if (ex != null)
            {
                log.Debug("Exception occurred - skipping operation auditing");
                return;
            }
            log.Debug("Processing incoming message");
            var now = DateTime.Now;
            var operationId = GetCurrentOperationId();
            log.DebugFormat("Current operation ID = {0}", operationId);
            if (string.IsNullOrEmpty(operationId))
                return;
            var operation = operationProvider.Find(operationId);
            if (operation == null)
            {
                log.WarnFormat("Unable to find an operation with ID = {0}. Some tracking " +
                    "information may be lost.", operationId);
                return;
            }
            Operation.Current = operation;
            var messageTypes = GetEnclosedMessageTypes();
            foreach (var receivedMessageType in messageTypes)
            {
                var typeName = receivedMessageType.FullName;
                var stage = new OperationStage(typeName, now);
                log.DebugFormat("Pushing operation stage, received message type = {0}",
                    typeName);
                operation.Push(stage);
            }
        }

        /// <inheritdoc />
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            log.Debug("Processing outgoing message");
            var operationId = GetCurrentOperationId();
            log.DebugFormat("Current operation ID = {0}", operationId);
            if (string.IsNullOrEmpty(operationId))
            {
                var originatingMessageType = messages[0].GetType().FullName;
                log.DebugFormat("Starting new operation with originating type = {0}",
                    originatingMessageType);
                var operation = operationProvider.Start(originatingMessageType);
                operationId = operation.Id;
                log.DebugFormat("New operation ID = {0}", operationId);
            }
            Operation.Current = operationProvider.Find(operationId);
            transportMessage.Headers[OperationIdHeader] = operationId;
        }

        private string GetCurrentOperationId()
        {
            log.Debug("Looking for current operation ID");
            if (bus.CurrentMessageContext == null)
            {
                log.Debug("No message context is active - operation ID is not available");
                return null;
            }
            string operationId;
            bus.CurrentMessageContext.Headers.TryGetValue(OperationIdHeader, out operationId);
            return operationId;
        }

        private IEnumerable<Type> GetEnclosedMessageTypes()
        {
            string enclosedMessageTypes;
            if (!bus.CurrentMessageContext.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes",
                out enclosedMessageTypes))
                yield break;
            var typeArray = enclosedMessageTypes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var typeName in typeArray)
            {
                var messageType = Type.GetType(typeName, false);
                if (messageType != null)
                    yield return messageType;
            }
        }
    }
}