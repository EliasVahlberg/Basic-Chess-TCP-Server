using System;
using System.Threading;

namespace ChessServer
{
    class Program
    {
        //! Depricated method

        // * Important ifo
        // ? Question

        //TODO 

        static readonly int version = 1;
        static public bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Chess Game Server build: " + version;
            Server.Start(64, 9600);
            isRunning = true;
            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
        }
        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Tickrate: {Constants.TICKS_PER_SECOND} ");
            DateTime _nextLoop = DateTime.Now;
            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
            System.Console.WriteLine("Stopping");
        }
    }
}
