using System.Collections.Generic;

namespace Better.BuildNotification.Platform.Tooling.RequestWrappers
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