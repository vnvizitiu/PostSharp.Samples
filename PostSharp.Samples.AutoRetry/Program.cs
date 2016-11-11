using System;
using System.Diagnostics;
using System.Net;

namespace PostSharp.Samples.AutoRetry
{
    internal static class Program
    {
        private static readonly Random random = new Random();
        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

        private static void Main(string[] args)
        {
            var content = DownloadFile();
            Console.WriteLine(content);
        }


        [AutoRetry(MaxRetries = 3)]
        private static string DownloadFile()
        {
            WriteMessage("Attempting to download the file.");

            // Randomly decide if the method call should succeed or fail.
            if (random.NextDouble() < 0.8)
            {
                // Simulate a network failure.

                WriteMessage("Network failure.");
                throw new WebException();
            }
            // Simulate success.
            WriteMessage("Success!");
            return "Hello, world.";
        }

        // Writes a message to the console with a timestamp.
        private static void WriteMessage(string message)
        {
            Console.WriteLine("{0} ms - {1}", stopwatch.ElapsedMilliseconds, message);
        }
    }
}