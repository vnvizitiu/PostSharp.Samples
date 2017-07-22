using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PostSharp.Samples.Caching
{
    class RedisServer : IDisposable
    {
        Process process;

        private RedisServer(Process process)
        {
            this.process = process;
        }

        public static RedisServer Start()
        {
            if (!Process.GetProcessesByName("redis-server").Any())
            {
                string configFile = Path.GetFullPath("redis.conf");

                Console.WriteLine("Starting Redis server with config file: " + configFile);

                return new RedisServer( Process.Start(@"..\..\..\..\packages\redis-64.3.0.503\tools\redis-server.exe", configFile) );
            }
            else
            {
                return new RedisServer(null);
            }

        }


        public void Dispose()
        {
            if ( this.process != null )
            {
                Console.WriteLine("Stopping Redis server.");
                this.process.Close();
                this.process = null;
            }
        }
    }

}
