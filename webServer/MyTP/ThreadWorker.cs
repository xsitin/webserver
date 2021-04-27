using System;
using System.Threading;

namespace webServer.MyTP
{
    public class ThreadWorker
    {
        public Thread _thread;
        public readonly bool FromThreadPool;
        private bool isRunning;
        private static TaskQueue tq = TaskQueue.GetInstance();
        public IThreadedTask CurrentTask;
        private static int idCounter;
        public readonly int id;

        public bool IsRunning
        {
            get => _thread.ThreadState == ThreadState.Running||_thread.ThreadState==ThreadState.WaitSleepJoin && isRunning;
            set => isRunning = value;
        }

        public ThreadWorker()
        {
            id = Interlocked.Increment(ref idCounter);
            FromThreadPool = true;
            _thread = new Thread(Start);
            _thread.IsBackground = false;
        }

        public ThreadWorker(IThreadedTask task)
        {
            CurrentTask = task;
            id = Interlocked.Increment(ref idCounter);
            
            FromThreadPool = false;
            IsRunning = true;
            _thread = new Thread(task.Run);
            
        }


        public void Run()
        {
            IsRunning = true;
            _thread.Start();
        }

        public void Stop()
        {
            IsRunning = false;
        }


        private void Start()
        {
            while (IsRunning)
            {
                IThreadedTask task;
                lock (tq)
                {
                    if (tq.Any())
                        task = tq.GetTask();

                    else
                    {
                        IsRunning = false;
                        tq.ContainTasks.WaitOne();
                        IsRunning = true;
                        continue;
                    }
                }

                CurrentTask = task;
                task.Run();
                CurrentTask = null;
            }
        }
    }
}