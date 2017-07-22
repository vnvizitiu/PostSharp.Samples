using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Audit;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PostSharp.Samples.Logging
{
    [Log(AttributeExclude = true)]
    class Program
    {
        
        static void Main(string[] args)
        {
            var backend = new Patterns.Diagnostics.Backends.Console.ConsoleLoggingBackend();
            backend.Options.Delimiter = " \u00A6 ";
            LoggingServices.DefaultBackend = backend ;

            LoggingServices.Formatters.Register(new FancyIntFormatter());
            AuditServices.RecordPublished += OnAuditRecordPublished;

            LoggedProgram.Execute();


        }

        private static void OnAuditRecordPublished(object sender, AuditRecordEventArgs e)
        {
            // Typically, you would write this into a database.
            Console.WriteLine("AUDIT: " + e.Record.Text);
        }

    }

    static class LoggedProgram
    {
        static readonly Logger logger = Logger.GetLogger();


        public static void Execute()
        {
            Fibonacci(5);


            Task.WaitAll(
                LoggedProgram.ReadAndHashAsync("https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pineisland_oli_2017026_lrg-crop.jpg"),
                LoggedProgram.ReadAndHashAsync("https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pia20521-1041.jpg"));

            var activity = logger.OpenActivity("Hashing images without logging");
            try
            {
                logger.Write(LogLevel.Info, "Disabling debug logging the WebUtil.");
                LoggingServices.DefaultBackend.GetSource(LoggingRoles.Tracing, typeof(LoggedProgram)).SetLevel(LogLevel.Info);
                Task.WaitAll(
                    LoggedProgram.ReadAndHashAsync("https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pineisland_oli_2017026_lrg-crop.jpg"),
                    LoggedProgram.ReadAndHashAsync("https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pia20521-1041.jpg"));

                activity.SetSuccess();
            }
            catch (Exception e)
            {
                activity.SetException(e);
                throw;
            }

            UpdateCustomer(5, new CustomerData { FirstName = "Joe", LastName = "Brick"});

        }

        public static int Fibonacci(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException();
            if (n == 0)
                return 0;
            if (n == 1)
                return 1;

            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        [Audit]
        public static void UpdateCustomer(int customerId, CustomerData customerData)
        {
            logger.Write(LogLevel.Info, "Let's pretend we're updating customer {CustomerId} with {Data}.", customerId, customerData);
        }

        public static async Task ReadAndHashAsync(string url)
        {
            var hashAlgorithm = HashAlgorithm.Create("SHA256");
            hashAlgorithm.Initialize();

            var webClient = new WebClient();
            var buffer = new byte[16 * 1024];

            logger.Write(LogLevel.Info, "Using a {BufferSize}-byte buffer.", buffer.Length);

            using (var stream = webClient.OpenRead(url))
            {
                int countRead;
                while ((countRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    logger.Write(LogLevel.Info, "Got {CountRead} bytes.", countRead);
                    hashAlgorithm.ComputeHash(buffer, 0, countRead);
                }
            }
        }
    }
}
