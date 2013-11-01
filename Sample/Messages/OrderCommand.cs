using System;

namespace NServiceBus.Tracking.Sample.Messages
{
    public class OrderCommand : ICommand
    {
        public string Description { get; set; }
    }
}