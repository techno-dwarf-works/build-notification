namespace Better.BuildNotification.Platform.Tooling
{
    public abstract class FirebaseDataLoader
    {
        public const string AssetExtensionWithDot = ".txt";
        protected internal static FirebaseDataLoader _instance;
        public static FirebaseDataLoader Instance => _instance;

        public abstract FirebaseData GetData();
        public abstract void SaveData(FirebaseData data);
        public abstract void SetKey(string encryptionKey);
        public abstract string GetCurrentKey();
        public abstract byte[] GetCurrentKeyBytes();
        public abstract void ClearCurrentKey();
        public abstract void GenerateNewKey();
        public abstract void DeleteData();
    }
}