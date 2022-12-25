using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public static class EditorMainThreadDispatched
    {
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
        private static readonly SemaphoreSlim ExecutionQueueLock = new SemaphoreSlim(1, 1);

        static EditorMainThreadDispatched()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            ExecutionQueueLock.Wait();

            try
            {
                while (ExecutionQueue.Count > 0) ExecutionQueue.Dequeue().Invoke();
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public static void Enqueue(Action action)
        {
            ExecutionQueueLock.Wait();

            try
            {
                ExecutionQueue.Enqueue(() => { action?.Invoke(); });
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }
    }
}