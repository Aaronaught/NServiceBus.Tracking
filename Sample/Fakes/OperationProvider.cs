using System;
using System.Collections.Generic;

namespace NServiceBus.Tracking.Sample.Fakes
{
    class OperationProvider : IOperationProvider
    {
        private readonly Dictionary<string, IOperation> operations = 
            new Dictionary<string, IOperation>();

        public IOperation Find(string id)
        {
            IOperation operation;
            operations.TryGetValue(id, out operation);
            return operation;
        }

        public IOperation Start(string originatingMessageType)
        {
            var id = Guid.NewGuid().ToString();
            var operation = new Operation(id, originatingMessageType, DateTime.Now);
            operations.Add(id, operation);
            return operation;
        }
    }
}
