using System;

namespace NServiceBus.Tracking.Sample.Messages
{
    public class BillCommand : ICommand
    {
        public int Id { get; set; }
    }
}