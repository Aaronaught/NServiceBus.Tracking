using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceBus.Tracking.Sample.Fakes
{
    class Operation : IOperation
    {
        private readonly string id;
        private readonly string originatingMessageType;
        private readonly DateTime whenStarted;
        private readonly List<string> completionMessageTypes = new List<string>();
        private readonly List<OperationStage> stages = new List<OperationStage>();

        public Operation(string id, string originatingMessageType, DateTime whenStarted)
        {
            this.id = id;
            this.originatingMessageType = originatingMessageType;
            this.whenStarted = whenStarted;
        }

        public void CompleteAfter(params string[] messageTypeNames)
        {
            completionMessageTypes.Clear();
            completionMessageTypes.AddRange(messageTypeNames);
        }

        public IEnumerable<OperationStage> GetHistory()
        {
            return stages;
        }

        public bool IsCompleted()
        {
            var receivedMessageTypes = (stages ?? Enumerable.Empty<OperationStage>())
                .Select(s => s.ReceivedMessageType);
            return !completionMessageTypes.Except(receivedMessageTypes).Any();
        }

        public void Push(OperationStage nextStage)
        {
            stages.Add(nextStage);
        }

        public bool WasMessageReceived(string messageTypeName)
        {
            return stages.Any(s => s.ReceivedMessageType == messageTypeName);
        }

        public string Id
        {
            get { return id; }
        }

        public string OriginatingMessageType
        {
            get { return originatingMessageType; }
        }

        public DateTime WhenStarted
        {
            get { return whenStarted; }
        }
    }
}