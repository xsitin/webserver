using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace webServer.MyTP
{
    public class ThreadDispatcher
    {
        private static readonly object locker = new();
        private static ThreadDispatcher instance;
        private readonly Thread cleaner = new(Clean);
        private static List<ThreadWorker> _workers = new();


        private ThreadDispatcher()
        {
            for (var i = 0; i < Environment.ProcessorCount * 2; i++) _workers.Add(new ThreadWorker());
            cleaner.IsBackground = true;
            cleaner.Start();
            AddInQueue(new ThreadMonitor(_workers));
        }

        public static ThreadDispatcher GetInstance()
        {
            lock (locker)
                return instance ??= new ThreadDispatcher();
        }

        private static void Clean()
        {
            while (true)
            {
                lock (_workers)
                    _workers.RemoveAll(x => !(x.FromThreadPool || x.IsRunning));
                Thread.Sleep(50);
            }
        }

        public void SetPoolSize(int count)
        {
            lock (_workers)
            {
                while (_workers.Count > count)
                {
                    _workers.Last().Stop();
                    _workers.Remove(_workers.Last());
                }

                while (_workers.Count < count)
                {
                    _workers.Add(new ThreadWorker());
                }
            }
        }

        public void Run()
        {
            lock (_workers)
            {
                for (var i = 0; i < _workers.Count; i++)
                {
                    Console.WriteLine(i);
                    Console.WriteLine(_workers[i].CurrentTask);
                    if (!_workers[i].IsRunning)
                        _workers[i].Run();
                }
            }
        }


        TaskQueue _queue = TaskQueue.GetInstance();

        public void Add(IThreadedTask task)
        {
            var worker = new ThreadWorker(task);
            worker.Run();
            lock (_workers)
            {
                _workers.Add(worker);
            }
        }

        public void AddInQueue(IThreadedTask task)
        {
            this._queue.AddTask(task);
        }
    }
}