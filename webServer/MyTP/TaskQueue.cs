using System.Collections.Generic;
using System.Threading;

namespace webServer.MyTP
{
    internal class TaskQueue
    {
        private static TaskQueue instance;
        protected Queue<IThreadedTask> _tasks = new();
        internal ManualResetEvent ContainTasks = new(true);

        protected TaskQueue()
        {
        }
        
        internal static TaskQueue GetInstance() => instance ??= new TaskQueue();
        
        internal bool Any()
        {
            lock (_tasks)
                return _tasks.Count > 0;
        }
        
        internal IThreadedTask GetTask()
        {
            lock (_tasks)
            {
                if (_tasks.Count<=1) ContainTasks.Reset();
                return _tasks.Dequeue();
            }
        }
        
        internal void AddTask(IThreadedTask task)
        {
            lock (_tasks) _tasks.Enqueue(task);
            ContainTasks.Set();
        }
    }
}