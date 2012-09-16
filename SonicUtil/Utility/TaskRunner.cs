using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonicUtil.Utility
{
    public class TaskRunner
    {
        private BlockingCollection<Task> _taskQueue = new BlockingCollection<Task>();

        public TaskRunner(int concurrentTasks)
        {
            for (int i = 0; i < concurrentTasks; ++i)
                Task.Factory.StartNew(Run);
        }

        public Task Add(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var task = new Task(action, cancellationToken);
            _taskQueue.Add(task);
            return task;
        }

        public Task<T> Enqueue<T> (Func<T> function, CancellationToken cancellationToken = default(CancellationToken))
        {
            var task = new Task<T>(function, cancellationToken);
            _taskQueue.Add(task);
            return task;
        }

        void Run()
        {
            foreach(var task in _taskQueue.GetConsumingEnumerable())
            {
                try
                {
                    if(!task.IsCanceled)
                        task.RunSynchronously();
                }
                catch (InvalidOperationException)
                {
                    // Empty on purpose
                }
            }
        }
    }
}
