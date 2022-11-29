using System.Collections.Generic;

namespace Better.BuildNotification.Runtime.Tooling.DatabaseModule
{
    public class ListWrapper<T> : Wrapper
    {
        public List<T> Data { get; }
        public ListWrapper(List<T> data)
        {
            Data = data;
        }
    }
}