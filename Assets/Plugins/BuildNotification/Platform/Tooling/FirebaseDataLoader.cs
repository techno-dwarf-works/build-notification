using System.Threading;

namespace Better.BuildNotification.Platform.Tooling
{
    public abstract class FirebaseDataLoader
    {
        protected SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public const string AssetExtensionWithDot = ".txt";
        protected internal static FirebaseDataLoader _instance;
        public static FirebaseDataLoader Instance => _instance;

        public abstract FirebaseData GetData();
        protected abstract void SaveDataInternal(FirebaseData data);

        public async void SaveData(FirebaseData data)
        {
            await _semaphore.WaitAsync();
            SaveDataInternal(data);
            _semaphore.Release();
        }
        
        public abstract void SetKey(string encryptionKey);
        public abstract string GetCurrentKey();
        public abstract byte[] GetCurrentKeyBytes();
        public abstract void ClearCurrentKey();
        public abstract void GenerateNewKey();
        public abstract void DeleteData();
    }
}