namespace Better.BuildNotification.Runtime.Tooling.FirebaseImplementation
{
    public abstract class FirebaseDataLoader
    {
        protected internal static FirebaseDataLoader _instance;
        public static FirebaseDataLoader Instance => _instance;

        public abstract FirebaseData GetData();
    }
}