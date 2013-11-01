using System;
using System.Threading;
using NServiceBus.Tracking.Sample.Messages;

namespace NServiceBus.Tracking.Sample.Handlers
{
    public class EverythingHandler :
        IHandleMessages<OrderCommand>,
        IHandleMessages<BillCommand>,
        IHandleMessages<ShipCommand>
    {
        private static int lastOrderNumber;

        private readonly IBus bus;

        public EverythingHandler(IBus bus)
        {
            if (bus == null)
                throw new ArgumentNullException("bus");
            this.bus = bus;
        }

        public void Handle(OrderCommand message)
        {
            var orderNumber = Interlocked.Increment(ref lastOrderNumber);
            Console.Out.WriteLine(ConsoleColor.Cyan, "Placing order {0}, description = '{1}'...",
                orderNumber, message.Description);
            Thread.Sleep(1000);
            Console.Out.WriteLine(ConsoleColor.Cyan, "Finished placing order {0}", orderNumber);
            bus.SendLocal(new BillCommand { Id = orderNumber });
        }

        public void Handle(BillCommand message)
        {
            Console.Out.WriteLine(ConsoleColor.Cyan, "Billing for order {0}...", message.Id);
            Thread.Sleep(2000);
            Console.Out.WriteLine(ConsoleColor.Cyan, "Finished billing for order {0}", message.Id);
            bus.SendLocal(new ShipCommand { Id = message.Id });
        }

        public void Handle(ShipCommand message)
        {
            Console.Out.WriteLine(ConsoleColor.Cyan, "Shipping order {0}", message.Id);
            Thread.Sleep(1500);
            Console.Out.WriteLine(ConsoleColor.Cyan, "Finished shipping order {0}", message.Id);
        }
    }
}