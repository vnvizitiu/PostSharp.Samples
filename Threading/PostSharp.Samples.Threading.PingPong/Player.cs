using System;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace PostSharp.Samples.Threading.PingPong
{
    [Actor]
    class Player
    {
        [Reference]
        private readonly ConsoleLogger logger;

        readonly string name;

        [Reference]
        readonly Random random = new Random();
        
        private readonly double skills;

        private int counter;
        

        public Player( ConsoleLogger logger, string name, double skills )
        {
            this.logger = logger;
            this.name = name;
            this.skills = skills;
        }

        [Reentrant]
        public async Task<int> GetCounter()
        {
            return this.counter;
        }

        [Reentrant]
        public async Task<Player> Ping( Player peer, ConsoleColor color )
        {
            this.logger.WriteLine( string.Format( "{0}.Ping( color={1} ) from thread {2}", this.name, color, Thread.CurrentThread.ManagedThreadId), color);

            if ( random.NextDouble() <= skills )
            {
                this.counter++;
                return await peer.Ping(this, color);
            }
            else
            {
                return peer;
            }
        }

        [ExplicitlySynchronized]
        public override string ToString()
        {
            // It is safe to return a read-only immutable field.
            return this.name;
        }

    }
}
