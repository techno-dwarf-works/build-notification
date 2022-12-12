using System;
using System.Threading;

namespace Better.BuildNotification.Platform.Tooling
{
    public abstract class FirebaseLogger
    {
        protected SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public const string AssetExtensionWithDot = ".txt";
        protected internal static FirebaseLogger _instance;
        public static FirebaseLogger Instance => _instance;

        public abstract void LogError(string error);
        public abstract void Log(string log);
        public abstract void LogException(Exception exception);
    }
}