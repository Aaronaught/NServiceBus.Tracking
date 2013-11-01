using System;
using System.Threading;
using log4net.Config;
using NServiceBus.Installation.Environments;
using NServiceBus.Tracking.Sample.Fakes;
using NServiceBus.Tracking.Sample.Messages;

namespace NServiceBus.Tracking.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var startedEvent = new ManualResetEvent(false);
            var startableBus = Configure.With()
                .DefaultBuilder()
                .MsmqTransport()
                    .IsTransactional(false)
                    .PurgeOnStartup(true)
                .UnicastBus()
                    .ImpersonateSender(false)
                .OperationTracking()
                .FakeOperationProvider()
                .RunCustomAction(() =>
                    Configure.Instance.ForInstallationOn<Windows>().Install())
                .CreateBus();
            startableBus.Started += (sender, e) => startedEvent.Set();
            var bus = startableBus.Start();
            startedEvent.WaitOne();
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Press any key to start a message chain, or ESC to exit.");
                Console.WriteLine();
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
                Console.WriteLine();
                Console.WriteLine();
                bus.SendLocal(new OrderCommand { Description = "My order" });
                var operation = Operation.Current;
                operation.CompleteAfter<BillCommand, ShipCommand>();
                Console.Out.WriteLine(ConsoleColor.Green, "Operation ID is {0}", operation.Id);
                Console.Out.WriteLine(ConsoleColor.Green, "Waiting for operation to complete...");
                int waitCount = 0;
                while (!operation.IsCompleted())
                {
                    Thread.Sleep(500);
                    Console.Out.WriteLine(ConsoleColor.Gray, "Still waiting - polled {0} times", ++waitCount);
                }
                Console.Out.WriteLine(ConsoleColor.Green, "Ship command was received - operation completed!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("---");
            }
        }
    }
}