namespace BuildNotification.Runtime.Tooling.DatabaseModule.RequestWrappers
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