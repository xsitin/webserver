using System;
using System.Collections.Generic;
using System.Threading;

namespace webServer.MyTP
{
    internal class ThreadMonitor : TaskQueue, IThreadedTask
    {
        private List<ThreadWorker> workers;

        internal ThreadMonitor(List<ThreadWorker> t)
        {
            workers = t;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.WriteLine("ID   |   Task    |   IsRunning   |   IsFromThreadPool");
                lock (workers)
                {
                    for (var index = 0; index < workers.Count; index++)
                    {
                        var worker = workers[index];
                        Console.WriteLine(
                            $"{worker.id}     |   {worker.CurrentTask}    |   {worker.IsRunning}     |    {worker.FromThreadPool}");
                    }
                }

                Thread.Sleep(300);
            }
        }
    }
}