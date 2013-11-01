using System;

namespace NServiceBus.Tracking.Sample.Messages
{
    public class ShipCommand : ICommand
    {
        public int Id { get; set; }
    }
}