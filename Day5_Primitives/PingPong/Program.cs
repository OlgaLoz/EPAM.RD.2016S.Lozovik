using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new ManualResetEventSlim(false);
            var pingEvent = new AutoResetEvent(false);
            var pongEvent = new AutoResetEvent(false);

            // TODO: Create a new cancellation token source.
            CancellationTokenSource cts = new CancellationTokenSource();

            // TODO: Assign an appropriate value to token variable.
            CancellationToken token = cts.Token; 

            Action ping = () =>
            {
                Console.WriteLine("ping: Waiting for start.");
                start.Wait(token);

                bool continueRunning = true;

                while (continueRunning)
                {
                    Console.WriteLine("ping!");

                    // TODO: write ping-pong functionality here using pingEvent and pongEvent here.
                    pingEvent.Set();
                    pongEvent.WaitOne();

                    Thread.Sleep(1000);

                    // TODO: Use cancellation token "token" internals here to set appropriate value.
                    continueRunning = !token.IsCancellationRequested; 
                }

                // TODO: Fix issue with blocked pong task.
                pongEvent.Reset();

                Console.WriteLine("ping: done");
            };

            Action pong = () =>
            {
                Console.WriteLine("pong: Waiting for start.");
                start.Wait(token);

                bool continueRunning = true;

                while (continueRunning)
                {
                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pingEvent.WaitOne();
                    Console.WriteLine("pong!");
                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pongEvent.Set();

                    Thread.Sleep(1000);

                    // TODO: Use cancellation token "token" internals here to set appropriate value.
                    continueRunning = !token.IsCancellationRequested;  
                }

                // TODO: Fix issue with blocked ping task.
                pingEvent.Reset();

                Console.WriteLine("pong: done");
            };

            Task.Run(ping, token);
            Task.Run(pong, token);

            Console.WriteLine("Press any key to start.");
            Console.WriteLine("After ping-pong game started, press any key to exit.");
            Console.ReadKey();
            start.Set();

            Console.ReadKey();
           
            // TODO: cancel both tasks using cancellation token.
            cts.Cancel();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
