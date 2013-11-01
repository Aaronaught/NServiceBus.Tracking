using System;
using System.IO;

namespace NServiceBus.Tracking.Sample
{
    static class ConsoleExtensions
    {
        private static readonly object sync = new Object();

        public static void WriteLine(this TextWriter writer, ConsoleColor color, string format, params object[] args)
        {
            lock (sync)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(format, args);
                Console.ResetColor();
            }
        }
    }
}