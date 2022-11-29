namespace Better.BuildNotification.Runtime.Tooling.DatabaseModule
{
    public class SingleWrapper<T> : Wrapper
    {
        public T Data { get; }
        public SingleWrapper(T data)
        {
            Data = data;
        }
    }
}